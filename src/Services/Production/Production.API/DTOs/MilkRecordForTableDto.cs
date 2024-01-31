namespace Production.API.DTOs;

public record MilkRecordForTableDto(
    DateOnly? CalvingDate,
    IEnumerable<MilkRecordDto> MilkRecords);
