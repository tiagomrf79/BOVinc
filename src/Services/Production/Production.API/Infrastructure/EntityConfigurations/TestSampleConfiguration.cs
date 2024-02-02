using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Production.API.Models;

namespace Production.API.Infrastructure.EntityConfigurations;

public class TestSampleConfiguration : IEntityTypeConfiguration<TestSample>
{
    public void Configure(EntityTypeBuilder<TestSample> builder)
    {
        builder.ToTable("TestSample");

        builder.HasKey(x => x.Id);

        // With HiLo, EF fetches a block of values from the database to use as IDs
        builder.Property(x => x.Id)
            .UseHiLo("test_sample_hilo")
            .IsRequired();

        builder.Property(x => x.Date)
            .IsRequired();

        builder.Property(x => x.AnimalId)
            .IsRequired();
    }
}
