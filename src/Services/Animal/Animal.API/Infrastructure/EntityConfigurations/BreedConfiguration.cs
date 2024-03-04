using Animal.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Animal.API.Infrastructure.EntityConfigurations;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("Breed");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

        builder.Property(x => x.GestationLength).IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.LastUpdatedAt).IsRequired();
    }
}
