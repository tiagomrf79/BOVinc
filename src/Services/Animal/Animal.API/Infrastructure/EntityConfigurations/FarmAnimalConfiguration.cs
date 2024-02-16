using Animal.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Animal.API.Infrastructure.EntityConfigurations;

public class FarmAnimalConfiguration : IEntityTypeConfiguration<FarmAnimal>
{
    public void Configure(EntityTypeBuilder<FarmAnimal> builder)
    {
        builder.ToTable("Animal");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();

        builder.Property(x => x.RegistrationId).HasMaxLength(50);

        builder.Property(x => x.Name).HasMaxLength(50);

        builder.HasOne(x => x.Dam).WithMany().HasForeignKey(x => x.Dam);

        builder.HasOne(x => x.Sire).WithMany().HasForeignKey(x => x.Sire);

        builder.HasOne(x => x.Sex).WithMany().HasForeignKey(x => x.Sex);
        builder.Property(x => x.Sex).IsRequired();

        builder.HasOne(x => x.Breed).WithMany().HasForeignKey(x => x.Breed);
        builder.Property(x => x.Breed).IsRequired();

        builder.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.Category);

        builder.HasOne(x => x.Purpose).WithMany().HasForeignKey(x => x.Purpose);

        builder.Property(x => x.IsActive).IsRequired();

        builder.HasOne(x => x.Catalog).WithMany().HasForeignKey(x => x.Catalog);
        builder.Property(x => x.Catalog).IsRequired();

        builder.Property(x => x.Notes).HasMaxLength(250);

        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.LastUpdatedAt).IsRequired();
    }
}
