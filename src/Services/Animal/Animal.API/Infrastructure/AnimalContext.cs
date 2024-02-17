using Animal.API.Enums;
using Animal.API.Infrastructure.EntityConfigurations;
using Animal.API.Infrastructure.ValueConventions;
using Animal.API.Infrastructure.ValueConverters;
using Animal.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Animal.API.Infrastructure;

public class AnimalContext : DbContext
{
    public DbSet<FarmAnimal> FarmAnimals { get; set; }
    public DbSet<Breed> Breeds { get; set; }
    public DbSet<Lactation> Lactations { get; set; }

    public DbSet<AnimalStatus> AnimalStatus { get; set; }

    public AnimalContext(DbContextOptions<AnimalContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new EnumerationConfiguration<BreedingStatus>());
        modelBuilder.ApplyConfiguration(new EnumerationConfiguration<Catalog>());
        modelBuilder.ApplyConfiguration(new EnumerationConfiguration<Category>());
        modelBuilder.ApplyConfiguration(new EnumerationConfiguration<MilkingStatus>());
        modelBuilder.ApplyConfiguration(new EnumerationConfiguration<Purpose>());
        modelBuilder.ApplyConfiguration(new EnumerationConfiguration<Sex>());

        modelBuilder.ApplyConfiguration(new AnimalStatusConfiguration());
        modelBuilder.ApplyConfiguration(new BreedConfiguration());
        modelBuilder.ApplyConfiguration(new FarmAnimalConfiguration());
        modelBuilder.ApplyConfiguration(new LactationConfiguration());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter, DateOnlyComparer>() //since we convert DateOnly to DateTime, we also want to prevent a complicated comparison
            .HaveColumnType("date"); //to map DateOnly to SQL date, instead of SQL datetime2
    }
}
