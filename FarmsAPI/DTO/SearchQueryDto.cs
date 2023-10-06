using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FarmsAPI.DTO;

public class SearchQueryDto<T> : IValidatableObject
{
    /// <summary>Index of the page to return</summary>
    [Range(0, int.MaxValue)]
    [DefaultValue(0)]
    public int PageIndex { get; set; } = 0;

    /// <summary>Number of records per page</summary>
    [Range(1, 100)]
    [DefaultValue(10)]
    public int PageSize { get; set; } = 10;

    /// <summary>Column for sort order</summary>
    [DefaultValue("Name")]
    public string? SortColumn { get; set; } = "Name";

    /// <summary>Sort order</summary>
    [DefaultValue("ASC")]
    public string? SortOrder { get; set; } = "ASC";

    /// <summary>Search string for name</summary>
    [DefaultValue(null)]
    public string? FilterQuery { get; set; } = null;


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> results = new();

        Type entityType = typeof(T);
        if (entityType != null)
        {
            if (!entityType.GetProperties().Any(p => p.Name.ToLower() == SortColumn!.ToLower()))
            {
                ValidationResult result = new("Value must match an existing column.", new[] { nameof(SortColumn) });
                results.Add(result);
            }
        }

        if (SortOrder != "ASC" && SortOrder != "DESC")
        {
            ValidationResult result = new("Value must be one of the following: ASC, DESC.", new[] { nameof(SortOrder) });
            results.Add(result);
        }

        return results;
    }
}
