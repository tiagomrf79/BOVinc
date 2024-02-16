namespace Animal.API.DTOs;

public record AnimalUpdateDto (
    int Id,
    string? RegistrationId,
    string? Name,
    DateOnly? DateOfBirth,
    int? DamId,
    int? SireId,
    int SexId,
    int BreedId,
    int? PurposeId,
    string? Notes
);