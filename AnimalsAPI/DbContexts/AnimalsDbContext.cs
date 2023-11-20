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
        modelBuilder.Entity<Animal>().Property(a => a.Gender).IsRequired().HasConversion(g => (int)g, g => (Gender)g);
        modelBuilder.Entity<Animal>().Property(a => a.Breed).IsRequired().HasConversion(b => (int)b, b => (Breed)b);
        modelBuilder.Entity<Animal>().Property(a => a.DateOfBirth).IsRequired();
        modelBuilder.Entity<Animal>().HasOne(a => a.Mother).WithMany().HasForeignKey(a => a.MotherId);
        modelBuilder.Entity<Animal>().HasOne(a => a.Father).WithMany().HasForeignKey(a => a.FatherId);
        modelBuilder.Entity<Animal>().Property(a => a.DateCreated).IsRequired();

    }
}
