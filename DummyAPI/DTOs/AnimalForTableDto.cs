namespace DummyAPI.DTOs;

public class AnimalForTableDto
{
    public int TotalCount {  get; set; }
    public IEnumerable<AnimalDto> Animals { get; set; } = Enumerable.Empty<AnimalDto>();
}
