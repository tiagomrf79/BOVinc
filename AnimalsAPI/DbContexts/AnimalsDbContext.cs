using AnimalsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimalsAPI.DbContexts;

public class AnimalsDbContext : DbContext
{
    public DbSet<Animal> Animals => Set<Animal>();

    public AnimalsDbContext(DbContextOptions<AnimalsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Animal>().HasKey(a => a.Id);
        modelBuilder.Entity<Animal>().Property(a => a.Id).IsRequired();
        modelBuilder.Entity<Animal>().Property(a => a.FarmId).IsRequired();
        modelBuilder.Entity<Animal>().Property(a => a.RegistrationId).HasMaxLength(25);
        modelBuilder.Entity<Animal>().Property(a => a.Name).HasMaxLength(25);
        modelBuilder.Entity<Animal>().Property(a => a.Tag).HasMaxLength(10);
        //bind enums
        modelBuilder.Entity<Animal>().Property(a => a.DataOfBirth).IsRequired();
        //bind mother and father
        modelBuilder.Entity<Animal>().Property(a => a.DateCreated).IsRequired();

    }
}
