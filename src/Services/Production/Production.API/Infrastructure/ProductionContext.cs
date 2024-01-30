using Microsoft.EntityFrameworkCore;
using Production.API.Infrastructure.EntityConfigurations;
using Production.API.Models;

namespace Production.API.Infrastructure;

public class ProductionContext : DbContext
{
    public DbSet<Lactation> Lactations { get; set; }
    public DbSet<MilkRecord> MilkRecords { get; set; }

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
    }
}
