using Microsoft.EntityFrameworkCore;
using Occurrence.API.Enums;
using Occurrence.API.Infrastructure.EntityConfigurations;
using Occurrence.API.Models;

namespace Occurrence.API.Infrastructure;

public class OccurrenceContext : DbContext
{
    public DbSet<Group> Groups { get; set; }
    public DbSet<Event> Events { get; set; }

    public OccurrenceContext(DbContextOptions<OccurrenceContext> options) : base()
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new EnumerationConfiguration<ReasonEnteredHerd>());
        modelBuilder.ApplyConfiguration(new EnumerationConfiguration<ReasonLeftHerd>());
        modelBuilder.ApplyConfiguration(new EnumerationConfiguration<EventType>());

        modelBuilder.ApplyConfiguration(new GroupConfiguration());
    }
}
