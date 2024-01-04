namespace DummyAPI.DTOs;

public class AnimalDto
{
    public int Id { get; set; }
    public string? RegistrationId { get; set; }
    public string? Name { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public int GenderId { get; set; }
    public string Gender { get; set; } = string.Empty;
    public int BreedId { get; set; }
    public string Breed { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string Category { get; set; } = string.Empty;
    public int? DamId { get; set; }
    public string? DamName { get; set; }
    public int? SireId { get; set; }
    public string? SireName { get; set; }
    public int? LactationNumber { get; set; }
    public DateOnly? LastCalvingDate { get; set; }
    public int? BreedingStatusId { get; set; }
    public string? BreedingStatus { get; set; }
    public DateOnly? LastBreedingDate { get; set; }
    public int? BreedingBullId { get; set; }
    public string? BreedingBullName {get; set; }
    public DateOnly? LastDryDate { get; set; }
    public DateOnly? DueDateForCalving { get; set; }
    //asdasdasd
}
