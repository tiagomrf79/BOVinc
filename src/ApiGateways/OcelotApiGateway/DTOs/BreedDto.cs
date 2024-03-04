namespace OcelotApiGateway.DTOs;

public record BreedDto(
    int? Id,
    string Name,
    int GestationLength
);