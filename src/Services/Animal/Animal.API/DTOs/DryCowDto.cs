namespace Animal.API.DTOs;

public record DryCowDto (
    int Id,
    string? RegistrationId,
    string? Name,
    int LactationNumber,
    string? BreedingBullName,
    DateOnly LastDryDate,
    DateOnly DueDateForCalving
);