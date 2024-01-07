using System.ComponentModel.Design;

namespace DummyAPI.DTOs;

public class AnimalStatusDto
{
    //herd status
    public bool Active { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? CurrentGroupName { get; set; }
    public DateOnly? DateLeftHerd { get; set; }
    public string? ReasonLeftHerd { get; set; }

    //milking status
    public int? LactationNumber { get; set; }
    public int? MilkingStatusId { get; set; }
    public string? MilkingStatus { get; set; } = string.Empty;
    public DateOnly? LastCalvingDate { get; set; }
    public DateOnly? ScheduledDryDate { get; set; }
    public DateOnly? DryDate { get; set; }

    //reproductive status
    public int? BreedingStatusId {  get; set; }
    public string? BreedingStatus { get; set; }
    public DateOnly? LastHeatDate { get; set; }
    public DateOnly? ExpectedHeatDate { get; set; }
    public DateOnly? LastBreedingDate { get; set; }
    public DateOnly? DueDate { get; set; }
}
