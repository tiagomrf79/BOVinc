namespace Production.API.DTOs;

public record MilkRecordForChartVm(
    DateOnly? CalvingDate,
    IEnumerable<YieldOnlyRecordDto> ActualMilkRecords,
    IEnumerable<YieldOnlyRecordDto> AdjustedMilkRecords);