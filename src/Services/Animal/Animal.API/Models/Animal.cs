using Animal.API.Enums;

namespace Animal.API.Models;

public class Animal
{
    public int Id { get; set; }
    public string? RegistrationId { get; set; }
    public string? Name { get; set; }
    
    public DateOnly? DateOfBirth { get; set; }
    public Animal? Dam {  get; set; }
    public Animal? Sire {  get; set; }

    public required Sex Sex { get; set; }
    public required Breed Breed { get; set; }

    public Category? Category { get; set; }
    public Purpose? Purpose { get; set; }

    public required bool IsActive { get; set; }
    public required Catalog Catalog { get; set; }

    public string? Notes { get; set; }
}
