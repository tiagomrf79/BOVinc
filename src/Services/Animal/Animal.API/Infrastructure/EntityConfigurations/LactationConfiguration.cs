using Animal.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Animal.API.Infrastructure.EntityConfigurations;

public class LactationConfiguration : IEntityTypeConfiguration<Lactation>
{
    public void Configure(EntityTypeBuilder<Lactation> builder)
    {
        builder.ToTable("Lactation");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();

        builder.Property(x => x.LactationNumber).IsRequired();

        builder.Property(x => x.CalvingDate).IsRequired();

        builder.HasOne(x => x.FarmAnimal).WithMany().HasForeignKey(x => x.FarmAnimalId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.LastUpdatedAt).IsRequired();
    }
}