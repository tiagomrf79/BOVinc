namespace Animal.API.DTOs;

public record AscendantDto (
    string AnimalLabel,
    int SexId,
    IEnumerable<AscendantDto> Parents
);