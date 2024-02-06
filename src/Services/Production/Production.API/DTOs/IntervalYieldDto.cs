namespace Production.API.DTOs;

public record IntervalYieldDto(
    int DaysInMilkAtStart,
    int DaysInMilkAtEnd,
    double AverageYield
);