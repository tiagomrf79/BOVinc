namespace Production.API.DTOs;

public record MilkRecordForChartDto(
    DateOnly? CalvingDate,
    IEnumerable<MilkOnlyRecordDto> ActualMilkRecords,
    IEnumerable<MilkOnlyRecordDto> AdjustedMilkRecords);