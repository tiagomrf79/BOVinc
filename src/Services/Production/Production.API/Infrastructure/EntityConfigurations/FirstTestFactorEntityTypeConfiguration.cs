using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Production.API.Models;

namespace Production.API.Infrastructure.EntityConfigurations;

public class FirstTestFactorEntityTypeConfiguration : IEntityTypeConfiguration<FirstTestFactor>
{
    public void Configure(EntityTypeBuilder<FirstTestFactor> builder)
    {
        builder.ToTable("FirstTestFactor");

        builder.HasKey(x => new { x.DayOfFirstSampleMin, x.DayOfFirstSampleMax, x.IsFirstlactationCow })
            .HasName("PK_FirstTestFactor");

        builder.Property(x => x.DayOfFirstSampleMin)
            .IsRequired();

        builder.Property(x => x.DayOfFirstSampleMax)
            .IsRequired();

        builder.Property(x => x.IsFirstlactationCow)
            .IsRequired();

        builder.Property(x => x.MilkFactor)
            .IsRequired();

        builder.Property(x => x.FatFactor)
            .IsRequired();

        builder.Property(x => x.ProteinFactor)
            .IsRequired();
    }
}
