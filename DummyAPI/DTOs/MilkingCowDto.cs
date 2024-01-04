namespace DummyAPI.DTOs;

public class MilkingCowDto
{
    public int Id { get; set; }
    public string? RegistrationId { get; set; }
    public string? Name { get; set; }
    public int LactationNumber { get; set; }
    public DateOnly LastCalvingDate { get; set; }
    public int BreedingStatusId { get; set; }
    public string BreedingStatus { get; set; } = string.Empty;
    public DateOnly? LastBreedingDate { get; set; }
    public DateOnly? DueDateForCalving { get; set; }
}
