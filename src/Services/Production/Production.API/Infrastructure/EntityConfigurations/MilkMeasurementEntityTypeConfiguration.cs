using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Production.API.Models;

namespace Production.API.Infrastructure.EntityConfigurations;

public class MilkMeasurementEntityTypeConfiguration : IEntityTypeConfiguration<MilkMeasurement>
{
    public void Configure(EntityTypeBuilder<MilkMeasurement> builder)
    {
        builder.ToTable("MilkMeasurement");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .UseHiLo("milk_measurement_hilo")
            .IsRequired();

        builder.Property(x => x.Date)
            .IsRequired();

        builder.Property(x => x.AnimalId)
            .IsRequired();
    }
}
