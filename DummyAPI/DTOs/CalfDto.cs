namespace DummyAPI.DTOs;

public class CalfDto
{
    public int Id { get; set; }
    public string? RegistrationId { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public int GenderId { get; set; }
    public string Gender { get; set; } = string.Empty;
    public int BreedId { get; set; }
    public string Breed { get; set; } = string.Empty;
    public int? DamId { get; set; }
    public string? DamName { get; set; }
    public int? SireId { get; set; }
    public bool? IsCatalogSire { get; set; }
    public string? SireName { get; set; }
}
