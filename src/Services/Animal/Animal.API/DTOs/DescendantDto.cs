namespace Animal.API.DTOs;

public record DescendantDto (
    int id,
    string? DescendantLabel,
    int SexId,
    bool IsActive,
    string? DamLabel,
    string? SireLabel
);