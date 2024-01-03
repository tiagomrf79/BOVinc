namespace DummyAPI.DTOs;

public class HeiferForTableDto
{
    public int BreedingStatusId { get; set; }
    public string BreedingStatus { get; set; } = string.Empty;
    public DateOnly LastBreedingDate { get; set; }
    public DateOnly DueDate { get; set; }

}
