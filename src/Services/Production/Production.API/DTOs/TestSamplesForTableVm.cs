namespace Production.API.DTOs;

public record TestSamplesForTableVm(
    DateOnly? CalvingDate,
    IEnumerable<FullSampleDto> TestSamples);
