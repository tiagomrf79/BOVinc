namespace Production.API.DTOs;

public record YieldOnlyRecordDto(
    int Id,
    DateOnly Date,
    double Yield);