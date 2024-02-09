namespace Animal.API.DTOs;

public record BreedDto(
    int? Id,
    string Name,
    int GestationLength
);