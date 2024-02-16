using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Occurrence.API.Models;

namespace Occurrence.API.Infrastructure.EntityConfigurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("Group");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();

        builder.Property(x =>x.Name).HasMaxLength(50).IsRequired();

        builder.Property(x => x.Description).HasMaxLength(250);

        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.LastUpdatedAt).IsRequired();
    }
}
