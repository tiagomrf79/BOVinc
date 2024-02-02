namespace Production.API.DTOs;

public record TotalsForTableVm(
    int Order,
    string Name,
    IEnumerable<TotalDto> Totals);