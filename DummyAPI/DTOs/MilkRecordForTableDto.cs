namespace DummyAPI.DTOs;

public class MilkRecordForTableDto
{
    public DateOnly CalvingDate {  get; set; }
    public IEnumerable<MilkRecordDto> MilkRecords { get; set; } = Enumerable.Empty<MilkRecordDto>();
}
