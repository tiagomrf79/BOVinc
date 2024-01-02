using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.SystemConsole.Themes;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Services.AddApplicationInsightsTelemetry();

builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithThreadId()
        .Enrich.WithExceptionDetails()
        .Enrich.WithProperty("Assembly", $"{Assembly.GetExecutingAssembly().GetName().Name}");
    if (context.HostingEnvironment.IsDevelopment())
    {
        loggerConfiguration.WriteTo.Console(
            outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}",
            restrictedToMinimumLevel: LogEventLevel.Verbose,
            theme: AnsiConsoleTheme.Code);
        loggerConfiguration.WriteTo.Seq("http://seq_in_dc:5341");
    }
    else
    {
        loggerConfiguration.WriteTo.ApplicationInsights(
            services.GetRequiredService<TelemetryConfiguration>(),
            TelemetryConverter.Traces);
    }
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

builder.Services.AddControllers((options) =>
{
    options.ReturnHttpNotAcceptable = true;
    options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
    options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
    options.Filters.Add(new ProducesAttribute("application/json"));
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen((options) =>
{
    options.SwaggerDoc("DummyOpenAPISpecification", new()
    {
        Version = "v1",
        Title = "Dummy API",
        Description = "Through this API you can access dummy DTOs for frontend."
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlFullPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    options.IncludeXmlComments(xmlFullPath);

    options.EnableAnnotations();
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDistributedMemoryCache();
}
else
{
    builder.Services.AddStackExchangeRedisCache((options) =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("RedisCache") ?? throw new InvalidOperationException("Connection string 'RedisCache' not found.");
    });
}

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI((options) =>
{
    options.SwaggerEndpoint("/swagger/DummyOpenAPISpecification/swagger.json", "Dummy API");
});

if (!app.Configuration.GetValue<bool>("UseDeveloperExceptionPage"))
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

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

app.UseSerilogRequestLogging();

app.Run();
