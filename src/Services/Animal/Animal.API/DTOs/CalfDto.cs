namespace Animal.API.DTOs;

public record CalfDto (
    int Id,
    string? RegistrationId,
    DateOnly DateOfBirth,
    int SexId,
    string Sex,
    int BreedId,
    string Breed,
    int? DamId,
    string? DamName,
    int? SireId,
    string? SireName
);