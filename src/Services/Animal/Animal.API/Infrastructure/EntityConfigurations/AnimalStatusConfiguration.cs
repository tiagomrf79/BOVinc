using Animal.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Animal.API.Infrastructure.EntityConfigurations;

public class AnimalStatusConfiguration : IEntityTypeConfiguration<AnimalStatus>
{
    public void Configure(EntityTypeBuilder<AnimalStatus> builder)
    {
        builder.ToTable("AnimalStatus");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();

        builder.HasOne(x => x.Animal).WithOne().HasForeignKey<AnimalStatus>(x => x.AnimalId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.CurrentGroupName).HasMaxLength(50);

        builder.Property(x => x.ReasonLeftHerd).HasMaxLength(50);

        builder.HasOne(x => x.MilkingStatus).WithMany().HasForeignKey(x => x.MilkingStatusId);

        builder.HasOne(x => x.BreedingStatus).WithMany().HasForeignKey(x => x.BreedingStatusId);

        builder.Property(x => x.LastBreedingBull).HasMaxLength(50);
    }
}
