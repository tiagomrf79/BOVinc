namespace Production.API.DTOs;

public record MilkRecordDto(
    int? Id,
    DateOnly date,
    double? MilkYield,
    double? FatPercentage,
    double? ProteinPercentage,
    int? SomaticCellCount,
    int AnimalId);
