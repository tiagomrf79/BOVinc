namespace Animal.API.DTOs;

public record MilkingCowDto (
    int Id,
    string? RegistrationId,
    string? Name,
    int LactationNumber,
    DateOnly LastCalvingDate,
    int BreedingStatusId,
    string BreedingStatus,
    DateOnly? LastBreedingDate,
    DateOnly? DueDateForCalving
);