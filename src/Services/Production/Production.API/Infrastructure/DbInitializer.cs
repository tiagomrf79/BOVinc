namespace Production.API.Infrastructure;

public static class DbInitializer
{
    public static void Seed(IApplicationBuilder applicationBuilder)
    {
        ProductionContext context = applicationBuilder.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ProductionContext>();

        if (!context.FirstTestFactors.Any())
        {

        }
    }
}
