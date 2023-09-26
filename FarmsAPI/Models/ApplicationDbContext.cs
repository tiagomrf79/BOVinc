using Microsoft.EntityFrameworkCore;

namespace FarmsAPI.Models;

public class ApplicationDbContext : DbContext
{
    public DbSet<Herd> Herds => Set<Herd>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Herd>().HasKey(h => h.Id);
        builder.Entity<Herd>().Property(h => h.Id).IsRequired();
        builder.Entity<Herd>().Property(h => h.Name).IsRequired().HasMaxLength(100);
        builder.Entity<Herd>().Property(h => h.Address).HasMaxLength(200);
        builder.Entity<Herd>().Property(h => h.City).HasMaxLength(50);
        builder.Entity<Herd>().Property(h => h.Region).HasMaxLength(50);
        builder.Entity<Herd>().Property(h => h.Country).HasMaxLength(50);
        builder.Entity<Herd>().Property(h => h.DateCreated).IsRequired();
        builder.Entity<Herd>().Property(h => h.DateUpdated).IsRequired();
    }
}
