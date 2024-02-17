using Animal.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace Animal.API.Models;

public class FarmAnimal : IValidatableObject
{
    public int Id { get; set; }
    public string? RegistrationId { get; set; }
    public string? Name { get; set; }
    
    public DateOnly? DateOfBirth { get; set; }
    public int? DamId { get; set; }
    public FarmAnimal? Dam {  get; set; }
    public int? SireId { get; set; }
    public FarmAnimal? Sire {  get; set; }

    public required int SexId { get; set; }
    public Sex Sex {  get; set; }
    public required int BreedId { get; set; }
    public Breed Breed { get; set; }

    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public int? PurposeId { get; set; }
    public Purpose? Purpose { get; set; }

    public required bool IsActive { get; set; }
    public required int CatalogId { get; set; }
    public Catalog Catalog { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Dam != null && Dam.Id == Id)
            yield return new ValidationResult("Animal cannot be its own dam.", new[] { nameof(Dam) });

        if (Sire != null && Sire.Id == Id)
            yield return new ValidationResult("Animal cannot be its own sire.", new[] { nameof(Dam) });

        if (Purpose != null && Purpose == Purpose.Milk && Sex == Sex.Male)
            yield return new ValidationResult("Male animal cannot have milk purpose.", new[] { nameof(Purpose) });

        if (Category != null && (Category == Category.DryCow || Category == Category.MilkingCow || Category == Category.Heifer) && Sex == Sex.Male)
            yield return new ValidationResult("Male animal cannot belong to heifer or cow categories.", new[] { nameof(Purpose) });

        if (Category != null && (Category == Category.Bull || Category == Category.Steer) && Sex == Sex.Female)
            yield return new ValidationResult("Female animal cannot belong to bull or steer categories.", new[] { nameof(Purpose) });

        if (Purpose != null && IsActive == false)
            yield return new ValidationResult("Inactive animals cannot have a purpose.", new[] { nameof(Purpose) });

        if (Category != null && IsActive == false)
            yield return new ValidationResult("Inactive animals cannot have a category.", new[] { nameof(Purpose) });
    }
}
