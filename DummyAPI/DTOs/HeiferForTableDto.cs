namespace DummyAPI.DTOs;

public class HeiferForTableDto
{
    public int TotalCount { get; set; }
    public IEnumerable<HeiferDto> Heifers { get; set; } = Enumerable.Empty<HeiferDto>();
}
