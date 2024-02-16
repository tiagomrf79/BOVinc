using Animal.API.Enums;
using Animal.API.Infrastructure.EntityConfigurations;
using Animal.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Animal.API.Infrastructure;

/* data that belongs to other microservices
HERD STATUS
left herd reason
left herd date
current group name
REPRODUCTIVE STATUS
+breeding status
+last breeding date
+due date for calving
+breeding bull id/name
last heat date
expected heat date
MILKING STATUS
+last calving date
+last dry date
milking status
scheduled dry date
*/

public class AnimalContext : DbContext
{
    public DbSet<FarmAnimal> FarmAnimals { get; set; }
    public DbSet<Breed> Breeds { get; set; }
    public DbSet<Lactation> Lactations { get; set; }

    public DbSet<AnimalStatus> AnimalStatus { get; set; }

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

        modelBuilder.ApplyConfiguration(new BreedConfiguration());
        modelBuilder.ApplyConfiguration(new FarmAnimalConfiguration());
        modelBuilder.ApplyConfiguration(new LactationConfiguration());
    }
}
