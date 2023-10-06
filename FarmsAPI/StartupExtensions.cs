using FarmsAPI.DbContexts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Exceptions;
using System.Reflection;

namespace FarmsAPI;

public static class StartupExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();

        builder.Host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration
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
            options.SwaggerDoc("FarmsOpenAPISpecification", new()
            {
                Title = "Farms API",
                Version = "1",
                Description = "Through this API you can access farms."
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlFullPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
            options.IncludeXmlComments(xmlFullPath);

            options.EnableAnnotations();
        });

        builder.Services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseSqlServer(builder.Configuration.GetConnectionString("FarmsDatabase"));
        });

        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

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

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI((options) =>
            {
                options.SwaggerEndpoint("/swagger/FarmsOpenAPISpecification/swagger.json", "Farms API");
            });
        }

        if (!app.Configuration.GetValue<bool>("UseDeveloperExceptionPage"))
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

        return app;
    }
}
