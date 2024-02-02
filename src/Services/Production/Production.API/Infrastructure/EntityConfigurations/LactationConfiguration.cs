using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Production.API.Models;

namespace Production.API.Infrastructure.EntityConfigurations;

public class LactationConfiguration : IEntityTypeConfiguration<Lactation>
{
    public void Configure(EntityTypeBuilder<Lactation> builder)
    {
        builder.ToTable("Lactation");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.CalvingDate)
            .IsRequired();

        builder.Property(x => x.AnimalId)
            .IsRequired();
    }
}

