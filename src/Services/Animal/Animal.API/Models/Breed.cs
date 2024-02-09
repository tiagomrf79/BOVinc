using System.ComponentModel.DataAnnotations;

namespace Animal.API.Models;

public class Breed : IValidatableObject
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int GestationLength { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set;}

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (GestationLength < 250 || GestationLength > 310)
            yield return new ValidationResult("Invalid value for gestation length.", new[] { nameof(GestationLength) });
    }
}
