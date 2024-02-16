namespace Animal.API.DTOs;

public record HeiferDto (
    int Id,
    string? RegistrationId,
    DateOnly DateOfBirth,
    int? BreedingStatusId,
    string? BreedingStatus,
    DateOnly? LastBreedingDate,
    DateOnly? DueDateForCalving
);