namespace DummyAPI.DTOs;

public class DryCowTableDto
{
    public int TotalCount { get; set; }
    public IEnumerable<DryCowDto> DryCows { get; set; } = Enumerable.Empty<DryCowDto>();
}
