namespace Production.API.DTOs;

public record MilkTotalForTableDto(
    int Order,
    string Name,
    IEnumerable<MilkTotalDto> MilkTotals);