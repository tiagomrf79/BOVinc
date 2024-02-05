using Microsoft.EntityFrameworkCore;
using Production.API.Infrastructure.EntityConfigurations;
using Production.API.Models;

namespace Production.API.Infrastructure;

public class ProductionContext : DbContext
{
    public DbSet<Lactation> Lactations { get; set; }
    public DbSet<TestSample> TestSamples { get; set; }

    public DbSet<YieldFactors> FirstTestFactors { get; set; }
    public DbSet<PeakTestFactor> PeakTestFactors { get; set; }
    public DbSet<LastTestFactor> LastTestFactors { get; set; }

    public ProductionContext(DbContextOptions<ProductionContext> options) : base()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new LactationConfiguration());
        modelBuilder.ApplyConfiguration(new TestSampleConfiguration());

        modelBuilder.ApplyConfiguration(new FirstTestFactorConfiguration());
        modelBuilder.ApplyConfiguration(new PeakTestFactorConfiguration());
        modelBuilder.ApplyConfiguration(new LastTestFactorConfiguration());
    }
}
