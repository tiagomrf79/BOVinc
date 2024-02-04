using Microsoft.EntityFrameworkCore;
using Production.API.Infrastructure;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<ProductionContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:ProductionContextConnection"]);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

DbInitializer.ImportFactors(app);

app.Run();
