namespace Production.API.DTOs;

public record MilkTotalDto(
    int MilkYield,
    int FatYield,
    int ProteinYield);