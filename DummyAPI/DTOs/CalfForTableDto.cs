namespace DummyAPI.DTOs;

public class CalfForTableDto
{
    public int TotalCount { get; set; }
    public IEnumerable<CalfDto> Calves { get; set; } = Enumerable.Empty<CalfDto>();
}
