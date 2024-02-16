using Animal.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Animal.API.Infrastructure.EntityConfigurations;

public class AnimalStatusConfiguration : IEntityTypeConfiguration<AnimalStatus>
{
    public void Configure(EntityTypeBuilder<AnimalStatus> builder)
    {
        builder.ToTable("AnimalStatus");

        builder.HasOne(x => x.Animal).WithOne();
        builder.Property(x => x.Animal).IsRequired();

        builder.Property(x => x.LastBreedingBull).HasMaxLength(50);
    }
}
