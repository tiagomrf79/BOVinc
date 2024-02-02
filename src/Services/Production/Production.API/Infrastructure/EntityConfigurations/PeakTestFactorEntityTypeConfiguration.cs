using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Production.API.Models;

namespace Production.API.Infrastructure.EntityConfigurations;

public class PeakTestFactorEntityTypeConfiguration : IEntityTypeConfiguration<PeakTestFactor>
{
    public void Configure(EntityTypeBuilder<PeakTestFactor> builder)
    {
        builder.ToTable("PeakTestFactor");

        builder.HasKey(x => new { x.DayOfFirstSampleMin, x.DayOfFirstSampleMax, x.TestIntervalMin, x.TestIntervalMax, x.IsFirstlactationCow })
            .HasName("PK_PeakTestFactor");

        builder.Property(x => x.DayOfFirstSampleMin)
            .IsRequired();

        builder.Property(x => x.DayOfFirstSampleMax)
            .IsRequired();

        builder.Property(x => x.TestIntervalMin)
            .IsRequired();

        builder.Property(x => x.TestIntervalMax)
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
