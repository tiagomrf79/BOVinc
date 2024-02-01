namespace Production.API.DTOs;

public record MilkTotalForTableVm(
    int Order,
    string Name,
    IEnumerable<YieldsTotalDto> MilkTotals);