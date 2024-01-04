namespace DummyAPI.DTOs;

public class CalfForTableDto
{
    public int TotalCount { get; set; }
    public IEnumerable<CalfDto> Animals { get; set; } = Enumerable.Empty<CalfDto>();
}
