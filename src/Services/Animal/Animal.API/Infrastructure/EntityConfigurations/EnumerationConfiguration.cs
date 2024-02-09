using Animal.API.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Animal.API.Infrastructure.EntityConfigurations;

public class EnumerationConfiguration<T> : IEntityTypeConfiguration<T> where T : Enumeration
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.ToTable(nameof(T));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName(nameof(T))
            .IsRequired();
    }
}