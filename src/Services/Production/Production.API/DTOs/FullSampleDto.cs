namespace Production.API.DTOs;

public record FullSampleDto(
    int? Id,
    DateOnly Date,
    double? MilkYield,
    double? FatPercentage,
    double? ProteinPercentage,
    int? SomaticCellCount,
    int AnimalId);
