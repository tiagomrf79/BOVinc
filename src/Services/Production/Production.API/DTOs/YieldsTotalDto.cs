namespace Production.API.DTOs;

public record YieldsTotalDto(
    int MilkYield,
    int FatYield,
    int ProteinYield);