namespace Animal.API.DTOs;

public record AnimalDto (
    int Id,
    string? RegistrationId,
    string? Name,
    DateOnly DateOfBirth,
    int SexId,
    string Sex,
    int BreedId,
    string Breed,
    int CategoryId,
    string Category
);