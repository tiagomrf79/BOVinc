namespace Production.API.DTOs;

public record MilkRecordForTableVm(
    DateOnly? CalvingDate,
    IEnumerable<FullRecordDto> MilkRecords);
