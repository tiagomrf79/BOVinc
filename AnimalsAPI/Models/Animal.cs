namespace AnimalsAPI.Models;

public class Animal
{
    public int Id { get; set; }
    public int FarmId { get; set; }

    public string? RegistrationId { get; set; }
    public string? Name { get; set; }
    public string? Tag { get; set; }
    public Gender Gender { get; set; }
    public Breed Breed { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public int? MotherId { get; set; }
    public Animal? Mother { get; set; }
    public int? FatherId { get; set; }
    public Animal? Father { get; set; }

    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}
