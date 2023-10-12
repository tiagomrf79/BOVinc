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

        //seed data added through migrations
        builder.Entity<Farm>().HasData(new Farm
        {
            Id = 1,
            Name= "Jorge Oliveira de Pacheco",
            Address = "Avenida Diogo Leite 70",
            City = "Porto",
            Region = "Porto",
            Country = "Portugal",
            DateCreated = new DateTime(2023, 10, 7, 15, 57, 36),
            DateUpdated = new DateTime(2023, 10, 11, 17, 51, 22)
        });
        builder.Entity<Farm>().HasData(new Farm
        {
            Id = 2,
            Name = "Eduarda Clara Moreira",
            Address = "Estrada Monumental 316",
            City = "Funchal",
            Region = "Madeira",
            Country = "Portugal",
            DateCreated = new DateTime(2023, 4, 3, 17, 32, 38),
            DateUpdated = new DateTime(2023, 4, 9, 15, 34, 34)
        });
        builder.Entity<Farm>().HasData(new Farm
        {
            Id = 3,
            Name = "Nuno Abreu Nascimento",
            Address = "Avenida Calouste Gulbenkian 22B",
            City = "Coimbra",
            Region = "Coimbra",
            Country = "Portugal",
            DateCreated = new DateTime(2023, 7, 15, 16, 42, 14)
        });
        builder.Entity<Farm>().HasData(new Farm
        {
            Id = 4,
            Name = "André Raúl Cardoso",
            Address = "Largo Senhora-A-Branca 144",
            City = "Braga",
            Region = "Braga",
            Country = "Portugal",
            DateCreated = new DateTime(2023, 8, 28, 15, 34, 34)
        });
    }
}
