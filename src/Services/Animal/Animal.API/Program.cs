using Animal.API.Infrastructure;
using Animal.API.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Formatting.Json;
using System.Reflection;

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

var builder = WebApplication.CreateBuilder(args);

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

builder.Host
    // Direct all log messages to Serilog
    .UseSerilog((context, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
        // Include the context in each log context
        configuration.Enrich.FromLogContext();
        // Enrich all logs with trace information
        configuration.Enrich.WithSpan();
        // Output log messages to the console
        if (context.HostingEnvironment.IsDevelopment())
            configuration.WriteTo.Console(
                // Include the trace Id in the development log output
                outputTemplate: @"{Timestamp:yyyy-MM-dd HH:mm:ss} {TraceId} {Level:u3} {Message}{NewLine}{Exception}");
        else
            configuration.WriteTo.Console(
                new JsonFormatter());
    });

builder.Services.AddControllers((options) =>
{
    options.ReturnHttpNotAcceptable = true;
    options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
    options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
    options.Filters.Add(new ProducesAttribute("application/json"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AnimalContext>(opt =>
{
    var databaseConnectionString = builder.Configuration.GetConnectionString("AnimalDb") ?? throw new InvalidOperationException("Connection string 'AnimalDb' not found.");
    opt.UseSqlServer(databaseConnectionString);
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
builder.Services.AddScoped<IAnimalStatusRepository, AnimalStatusRepository>();
builder.Services.AddScoped<IBreedRepository, BreedRepository>();
builder.Services.AddScoped<ILactationRepository, LactationRepository>();

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

var app = builder.Build();

//TODO: log start of seeding database

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    try
    {
        var animalContext = scopedProvider.GetRequiredService<AnimalContext>();
        await AnimalContextSeed.SeedAsync(animalContext);
    }
    catch (Exception ex)
    {
        //TODO: log error while seeding the DB
    }
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.UseSerilogRequestLogging();

app.Run();
