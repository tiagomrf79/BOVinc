namespace DummyAPI.DTOs;

public class BullForTableDto
{
    public int TotalCount { get; set; }
    public IEnumerable<BullDto> Bulls { get; set; } = Enumerable.Empty<BullDto>();
}
