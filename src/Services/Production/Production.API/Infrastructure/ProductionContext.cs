using Microsoft.EntityFrameworkCore;
using Production.API.Infrastructure.EntityConfigurations;
using Production.API.Models;

namespace Production.API.Infrastructure;

public class ProductionContext : DbContext
{
    public DbSet<Lactation> Lactations { get; set; }
    public DbSet<TestSample> TestSamples { get; set; }

    public ProductionContext(DbContextOptions<ProductionContext> options) : base()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new LactationConfiguration());
        modelBuilder.ApplyConfiguration(new TestSampleConfiguration());
    }
}
