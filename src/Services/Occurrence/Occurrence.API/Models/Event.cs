using Occurrence.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace Occurrence.API.Models;

public class Event : IValidatableObject
{
    public int Id { get; set; }
    public required EventType EventType { get; set; }
    public DateOnly Date {  get; set; }
    public int AnimalId { get; set; }
    public string? Notes { get; set; }

    public int? BreedingBullId { get; set; }
    public string? BreedingBull {  get; set; }

    public Group? PreviousGroup { get; set; }
    public Group? NewGroup { get; set; }

    public ReasonEnteredHerd? ReasonEnteredHerd { get; set; }

    public ReasonLeftHerd? ReasonLeftHerd { get; set; }


    public string EventDescription
    {
        get
        {
            if (EventType == EventType.Breeding)
            {
                return $"Artificial Insemination with bull {BreedingBull}.";
            }
            else if (EventType == EventType.ChangedGroup)
            {
                return $"Move from {PreviousGroup!.Name} group to {NewGroup!.Name} group.";
            }

            return "";
        }
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EventType == EventType.ChangedGroup && PreviousGroup == null)
            yield return new ValidationResult("Previous group is mandatory.", new[] { nameof(PreviousGroup) });
        if (EventType == EventType.ChangedGroup && NewGroup == null)
            yield return new ValidationResult("New group is mandatory.", new[] { nameof(NewGroup) });

        if (EventType == EventType.EnteredHerd && ReasonEnteredHerd == null)
            yield return new ValidationResult("Reason for entering herd is mandatory.", new[] { nameof(ReasonEnteredHerd) });

        if (EventType == EventType.LeftHerd && ReasonLeftHerd == null)
            yield return new ValidationResult("Reason for leaving herd is mandatory.", new[] { nameof(ReasonLeftHerd) });
    }
}
