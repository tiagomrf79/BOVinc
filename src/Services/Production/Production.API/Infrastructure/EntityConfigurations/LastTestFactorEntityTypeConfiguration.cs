using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Production.API.Models;

namespace Production.API.Infrastructure.EntityConfigurations;

public class LastTestFactorEntityTypeConfiguration : IEntityTypeConfiguration<LastTestFactor>
{
    public void Configure(EntityTypeBuilder<LastTestFactor> builder)
    {
        builder.ToTable("LastTestFactor");

        builder.HasKey(x => new { x.DayOfLastSampleMin, x.DayOfLastSampleMax, x.TestIntervalMin, x.TestIntervalMax, x.IsFirstlactationCow })
            .HasName("PK_LastTestFactor");

        builder.Property(x => x.DayOfLastSampleMin)
            .IsRequired();

        builder.Property(x => x.DayOfLastSampleMax)
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
