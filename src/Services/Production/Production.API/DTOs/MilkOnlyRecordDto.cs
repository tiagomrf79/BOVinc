namespace Production.API.DTOs;

public record MilkOnlyRecordDto(
    int Id,
    DateOnly Date,
    double MilkYield);