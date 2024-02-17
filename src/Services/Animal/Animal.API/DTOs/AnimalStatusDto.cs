namespace Animal.API.DTOs;

public record AnimalStatusDto (
    int Id,
    bool IsActive,
    DateOnly? DateOfBirth,
    string? CurrentGroupName,
    DateOnly? DateLeftHerd,
    string? ReasonLeftHerd,
    int? LactationNumber,
    int? MilkingStatusId,
    string? MilkingStatus,
    DateOnly? LastCalvingDate,
    DateOnly? ScheduledDryDate,
    DateOnly? LastDryDate,
    int? BreedingStatusId,
    string? BreedingStatus,
    DateOnly? LastHeatDate,
    DateOnly? ExpectedHeatDate,
    DateOnly? LastBreedingDate,
    string? LastBreedingBull,
    DateOnly? DueDateForCalving
);
