namespace Production.API.DTOs;

public record YieldOnlySampleDto(
    int Id,
    DateOnly Date,
    double Yield);