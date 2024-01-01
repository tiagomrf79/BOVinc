namespace DummyAPI.DTOs;

public class MilkRecordsForTableDto
{
    public DateOnly CalvingDate {  get; set; }
    public IEnumerable<MilkRecordsDto> MilkRecords { get; set; } = Enumerable.Empty<MilkRecordsDto>();
}
