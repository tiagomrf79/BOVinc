namespace Animal.API.DTOs;

public record AnimalDetailDto (
    int Id,
    string? RegistrationId,
    string? Name,
    DateOnly? DateOfBirth,
    int? DamId,
    int? SireId,
    int SexId,
    int BreedId,
    int? CategoryId,
    int? PurposeId,
    bool IsActive,
    int CatalogId,
    string? Notes,

    int? LastLactationNumber
);