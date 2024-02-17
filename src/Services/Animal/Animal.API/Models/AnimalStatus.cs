using Animal.API.Enums;

namespace Animal.API.Models;

public class AnimalStatus
{
    public FarmAnimal Animal { get; set; }
    public string? CurrentGroupName { get; set; }
    public DateOnly? DateLeftHerd { get; set; }
    public string? ReasonLeftHerd { get; set; }

    public MilkingStatus? MilkingStatus { get; set; }
    public DateOnly? LastCalvingDate { get; set; }
    public DateOnly? SheduledDryDate { get; set; }
    public DateOnly? LastDryDate { get; set; }

    public BreedingStatus? BreedingStatus { get; set; }
    public DateOnly? LastHeatDate { get; set; }
    public DateOnly? ExpectedHeatDate { get; set; }
    public DateOnly? LastBreedingDate { get; set; }
    public string? LastBreedingBull { get; set; }
    public DateOnly? DueDateForCalving {  get; set; }
}
