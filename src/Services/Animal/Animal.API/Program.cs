using Animal.API.Infrastructure;
using Animal.API.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

var builder = WebApplication.CreateBuilder(args);

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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

app.Run();
