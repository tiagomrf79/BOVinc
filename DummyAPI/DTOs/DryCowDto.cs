namespace DummyAPI.DTOs;

public class DryCowDto
{
    public int Id { get; set; }
    public string? RegistrationId { get; set; }
    public string? Name { get; set; }
    public int LactationNumber { get; set; }
    public int? BreedingBullId { get; set; }
    public bool? IsCatalogSire { get; set; }
    public string? BreedingBullName { get; set; }
    public DateOnly LastDryDate { get; set; }
    public DateOnly DueDateForCalving { get; set; }
}
