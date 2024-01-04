using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DummyAPI.DTOs;

public class AnimalQueryDto<T> : IValidatableObject
{
    /// <summary>Search string</summary>
    [DefaultValue(null)]
    public string? SearchKeyword { get; set; } = null;


    /// <summary>Filter for gender</summary>
    [Required]
    [DefaultValue(0)]
    public int GenderFilter { get; set; } = 0;

    /// <summary>Filter for breed</summary>
    [Required]
    [DefaultValue(0)]
    public int BreedFilter { get; set; } = 0;


    /// <summary>Initial index to start pagination</summary>
    [Required]
    [Range(0, int.MaxValue)]
    [DefaultValue(0)]
    public int StartIndex { get; set; } = 0;

    /// <summary>Number of records per page</summary>
    [Required]
    [Range(1, 100)]
    [DefaultValue(10)]
    public int MaxRecords { get; set; } = 10;


    /// <summary>Sort attribute</summary>
    [Required]
    [DefaultValue("Name")]
    public string SortAttribute { get; set; } = "RegistrationId";

    /// <summary>Sort order</summary>
    [Required]
    [DefaultValue("ASC")]
    public string SortDirection { get; set; } = "ASC";


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> results = new();

        Type entityType = typeof(T);
        if (entityType != null)
        {
            if (!entityType.GetProperties().Any(p => p.Name.ToLower() == SortAttribute!.ToLower()))
            {
                ValidationResult result = new("Value must match an existing column.", new[] { nameof(SortAttribute) });
                results.Add(result);
            }
        }

        if (SortDirection != "ASC" && SortDirection != "DESC")
        {
            ValidationResult result = new("Value must be one of the following: ASC, DESC.", new[] { nameof(SortDirection) });
            results.Add(result);
        }

        return results;
    }
}