using FluentValidation;
using HerdsAPI.DbContexts;
using HerdsAPI.Models;
using HerdsAPI.Validations;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Exceptions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Logging.ClearProviders();

builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithThreadId()
        .Enrich.WithExceptionDetails()
        .Enrich.WithProperty("Assembly", $"{Assembly.GetExecutingAssembly().GetName().Name}");
});



builder.Services.AddCors((options) =>
{
    options.AddDefaultPolicy(cfg =>
    {
        cfg.WithOrigins(builder.Configuration["AllowedOrigins"]!);
        cfg.AllowAnyHeader();
        cfg.AllowAnyMethod();
    });
    options.AddPolicy(name: "AnyOrigin",
        cfg =>
        {
            cfg.AllowAnyOrigin();
            cfg.AllowAnyHeader();
            cfg.AllowAnyMethod();
        });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen((options) =>
{
    options.EnableAnnotations();
});

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("HerdsDatabase"));
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<IValidator<Herd>, HerdValidator>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDistributedMemoryCache();
}
else
{
    builder.Services.AddDistributedRedisCache(option =>
    {
        option.Configuration = builder.Configuration.GetConnectionString("RedisCache");
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Configuration.GetValue<bool>("UseDeveloperExceptionPage"))
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapGet("/error",
    [EnableCors("AnyOrigin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    (HttpContext context) =>
    {
        var exceptionHandler = context.Features.Get<IExceptionHandlerPathFeature>();

        var details = new ProblemDetails();
        details.Detail = exceptionHandler?.Error.Message;
        details.Extensions["traceId"] = System.Diagnostics.Activity.Current?.Id ?? context.TraceIdentifier;
        details.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
        details.Status = StatusCodes.Status500InternalServerError;

        app.Logger.LogError(exceptionHandler?.Error, "An unhandled exception occurred.");

        return Results.Problem(details);
    }).ExcludeFromDescription();


app.MapControllers();

app.Run();
