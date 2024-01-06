namespace DummyAPI.DTOs;

public class AnimalDetailDto
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public string? RegistrationId { get; set; }
    public string? Name { get; set; }
    public int BreedId { get; set; }
    public string Breed { get; set; } = string.Empty;
    public int GenderId { get; set; }
    public string Gender { get; set; } = string.Empty;
    public DateOnly? DateOfBirth { get; set; }
    public int? DamId { get; set; }
    public string? DamLabel { get; set; }
    public int? SireId { get; set; }
    public string? SireLabel { get; set; }
    public int InventorySourceId { get; set; }
    public string InventorySource { get; set; } = string.Empty;
    public int? LastLactationNumber { get; set; }
    public int? PurposeId { get; set; }
    public string? Purpose { get; set; }
    public string? Notes { get; set; }
}
