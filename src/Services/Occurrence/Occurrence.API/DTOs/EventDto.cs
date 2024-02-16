namespace Occurrence.API.DTOs;

public class EventDto
{
    public int? Id { get; set; }
    public int EventTypeId { get; set; }
    public string? EventType { get; set; }
    public DateOnly Date { get; set; }
    public int AnimalId { get; set; }
    public string? Notes { get; set; }

    public int? BreedingBullId { get; set; }
    public string? BreedingBull { get; set; }

    public int? PreviousGroupId { get; set; }
    public string? PreviousGroupName { get; set; }
    public int? NewGroupId { get; set; }
    public string? NewGroupName { get; set; }

    public int? ReasonEnteredHerdId { get; set; }
    public string? ReasonEnteredHerd { get; set; }

    public int? ReasonLeftHerdId { get; set; }
    public string? ReasonLeftHerd { get; set; }

    public string? EventDescription { get; set; }
}
