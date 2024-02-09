using Animal.API.Enums;
using Animal.API.Infrastructure.EntityConfigurations;
using Animal.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Animal.API.Infrastructure;

public class AnimalContext : DbContext
{
    public DbSet<FarmAnimal> FarmAnimals { get; set; }
    public DbSet<Breed> Breeds { get; set; }
    public DbSet<Lactation> Lactations { get; set; }

    public DbSet<Catalog> CatalogItems { get; set; }
    public DbSet<Category> CategoryItems { get; set; }
    public DbSet<Purpose> PurposeItems { get; set; }
    public DbSet<Sex> SexItems { get; set; }

    public AnimalContext(DbContextOptions<AnimalContext> options) : base()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new EnumerationConfiguration<Catalog>());
        modelBuilder.ApplyConfiguration(new EnumerationConfiguration<Category>());
        modelBuilder.ApplyConfiguration(new EnumerationConfiguration<Purpose>());
        modelBuilder.ApplyConfiguration(new EnumerationConfiguration<Sex>());

        modelBuilder.ApplyConfiguration(new FarmAnimalConfiguration());
    }
}
