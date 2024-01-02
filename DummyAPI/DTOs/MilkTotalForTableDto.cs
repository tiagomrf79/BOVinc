namespace DummyAPI.DTOs;

public class MilkTotalForTableDto
{
    public int Order {  get; set; }
    public string Name { get; set; } = string.Empty;
    public IEnumerable<MilkTotalDto> MilkTotals { get; set; } = Enumerable.Empty<MilkTotalDto>();
}
