namespace DummyAPI.DTOs;

public class BullDto
{
    public int Id { get; set; }
    public string? RegistrationId { get; set; }
    public string? Name { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public int BreedId { get; set; }
    public string Breed { get; set; } = string.Empty;
}
