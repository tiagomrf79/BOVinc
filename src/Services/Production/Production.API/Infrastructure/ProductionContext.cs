using Microsoft.EntityFrameworkCore;
using Production.API.Infrastructure.EntityConfigurations;
using Production.API.Models;

namespace Production.API.Infrastructure;

public class ProductionContext : DbContext
{
    public DbSet<Lactation> Lactations { get; set; }
    public DbSet<MilkRecord> MilkRecords { get; set; }

    public DbSet<FirstTestFactor> FirstTestFactors { get; set; }
    public DbSet<PeakTestFactor> PeakTestFactors { get; set; }
    public DbSet<LastTestFactor> LastTestFactors { get; set; }

    public ProductionContext(DbContextOptions<ProductionContext> options) : base()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(
            new LactationEntityTypeConfiguration());

        modelBuilder.ApplyConfiguration(
            new MilkRecordEntityTypeConfiguration());

        modelBuilder.ApplyConfiguration(
            new FirstTestFactorEntityTypeConfiguration());

        modelBuilder.ApplyConfiguration(
            new PeakTestFactorEntityTypeConfiguration());

        modelBuilder.ApplyConfiguration(
            new LastTestFactorEntityTypeConfiguration());
    }
}
