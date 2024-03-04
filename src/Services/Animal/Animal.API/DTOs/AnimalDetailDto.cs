namespace Animal.API.DTOs;

public record AnimalDetailDto
{
    public int Id { get; init; }
    public string? RegistrationId { get; init; }
    public string? Name { get; init; }
    public DateOnly? DateOfBirth { get; init; }
    public int? DamId { get; init; }
    public int? SireId { get; init; }
    public int SexId { get; init; }
    public int BreedId { get; init; }
    public int? CategoryId { get; init; }
    public int? PurposeId { get; init; }
    public bool IsActive { get; init; }
    public int CatalogId { get; init; }
    public string? Notes { get; init; }

    public int? LastLactationNumber { get; init; }
}