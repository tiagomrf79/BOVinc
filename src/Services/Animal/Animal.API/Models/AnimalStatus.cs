using Animal.API.Enums;

namespace Animal.API.Models;

public class AnimalStatus
{
    public FarmAnimal Animal { get; set; }
    public BreedingStatus? BreedingStatus { get; set; }
    public DateOnly? LastBreedingDate { get; set; }
    public string? LastBreedingBull { get; set; }
    public DateOnly? LastCalvingDate { get; set; }
    public DateOnly? DueDateForCalving {  get; set; }
    public DateOnly? LastDryDate {  get; set; }
}
