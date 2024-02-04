namespace Production.API.DTOs;

public record TestSamplesForChartVm(
    DateOnly? CalvingDate,
    IEnumerable<YieldOnlySampleDto> TestSamples,
    IEnumerable<YieldOnlySampleDto> TestSamplesWithPrediction);