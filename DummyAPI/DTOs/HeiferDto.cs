namespace DummyAPI.DTOs;

public class HeiferDto
{
    public int Id { get; set; }
    public string? RegistrationId { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public int? BreedingStatusId { get; set; }
    public string? BreedingStatus { get; set; }
    public DateOnly? LastBreedingDate { get; set; }
    public DateOnly? DueDateForCalving { get; set; }
}
