namespace DummyAPI.DTOs;

public class MilkingCowForTableDto
{
    public int TotalCount { get; set; }
    public IEnumerable<MilkingCowDto> MilkingCows { get; set; } = Enumerable.Empty<MilkingCowDto>();
}
