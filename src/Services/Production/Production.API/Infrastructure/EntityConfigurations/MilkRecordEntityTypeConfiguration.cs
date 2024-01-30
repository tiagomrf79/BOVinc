using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Production.API.Models;

namespace Production.API.Infrastructure.EntityConfigurations;

public class MilkRecordEntityTypeConfiguration : IEntityTypeConfiguration<MilkRecord>
{
    public void Configure(EntityTypeBuilder<MilkRecord> builder)
    {
        builder.ToTable("MilkRecord");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .UseHiLo("milk_record_hilo")
            .IsRequired();

        builder.Property(x => x.Date)
            .IsRequired();

        builder.Property(x => x.AnimalId)
            .IsRequired();
    }
}
