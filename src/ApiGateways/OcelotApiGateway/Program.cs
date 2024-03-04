using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OcelotApiGateway.Aggregators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration).AddSingletonDefinedAggregator<TestAggregator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//https://arbems.com/en/building-api-gateway-on-net-with-ocelot/#Request-Aggregation-in-API-gateways-with-Ocelot
await app.UseOcelot();

app.Run();
