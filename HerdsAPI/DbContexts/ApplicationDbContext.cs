using FarmsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmsAPI.DbContexts;

public class ApplicationDbContext : DbContext
{
    public DbSet<Farm> Farms => Set<Farm>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Farm>().HasKey(h => h.Id);
        builder.Entity<Farm>().Property(h => h.Id).IsRequired();
        builder.Entity<Farm>().Property(h => h.Name).IsRequired().HasMaxLength(100);
        builder.Entity<Farm>().Property(h => h.Address).HasMaxLength(200);
        builder.Entity<Farm>().Property(h => h.City).HasMaxLength(50);
        builder.Entity<Farm>().Property(h => h.Region).HasMaxLength(50);
        builder.Entity<Farm>().Property(h => h.Country).HasMaxLength(50);
        builder.Entity<Farm>().Property(h => h.DateCreated).IsRequired();
    }
}
