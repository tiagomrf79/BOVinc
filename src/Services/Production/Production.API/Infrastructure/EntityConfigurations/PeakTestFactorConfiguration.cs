﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Production.API.Models;

namespace Production.API.Infrastructure.EntityConfigurations;

public class PeakTestFactorConfiguration : IEntityTypeConfiguration<PeakTestFactor>
{
    public void Configure(EntityTypeBuilder<PeakTestFactor> builder)
    {
        builder.ToTable("PeakTestFactor");

        builder.HasKey(x => new { x.DayOfPreviousSampleMin, x.DayOfPreviousSampleMax, x.TestIntervalMin, x.TestIntervalMax, x.IsFirstlactation })
            .HasName("PK_PeakTestFactor");

        builder.Property(x => x.DayOfPreviousSampleMin)
            .IsRequired();

        builder.Property(x => x.DayOfPreviousSampleMax)
            .IsRequired();

        builder.Property(x => x.TestIntervalMin)
            .IsRequired();

        builder.Property(x => x.TestIntervalMax)
            .IsRequired();

        builder.Property(x => x.IsFirstlactation)
            .IsRequired();

        builder.Property(x => x.MilkFactor)
            .IsRequired();

        builder.Property(x => x.FatFactor)
            .IsRequired();

        builder.Property(x => x.ProteinFactor)
            .IsRequired();
    }
}
