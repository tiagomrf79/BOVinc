﻿using System.ComponentModel.DataAnnotations;

namespace Production.API.Models;

public class TestSample : IValidatableObject
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public double MilkYield { get; set; }
    public double? FatPercentage { get; set; }
    public double? ProteinPercentage {  get; set; }
    public int? SomaticCellCount { get; set; }
    public int AnimalId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set;}

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Date > DateOnly.FromDateTime(DateTime.UtcNow))
            yield return new ValidationResult("Date of measurement cannot be in the future.", new[] { nameof(Date) });

        if (MilkYield < 0)
            yield return new ValidationResult("Milk yield cannot be negative.", new[] { nameof(MilkYield) });

        if (FatPercentage != null & FatPercentage < 0)
            yield return new ValidationResult("Fat percentage cannot be negative.", new[] { nameof(FatPercentage) });

        if (ProteinPercentage != null & ProteinPercentage < 0)
            yield return new ValidationResult("Protein percentage cannot be negative.", new[] { nameof(ProteinPercentage) });

        if (SomaticCellCount != null & SomaticCellCount < 0)
            yield return new ValidationResult("Somatic cell count cannot be negative.", new[] { nameof(SomaticCellCount) });
    }
}
