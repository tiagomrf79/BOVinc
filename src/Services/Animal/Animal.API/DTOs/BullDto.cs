namespace Animal.API.DTOs;

public record BullDto (
    int Id,
    string? RegistrationId,
    string? Name,
    DateOnly DateOfBirth,
    int BreedId,
    string Breed
);