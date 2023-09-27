using System.ComponentModel.DataAnnotations;

namespace HerdsAPI.DTO;

public class SearchQueryDto<T> : IValidatableObject
{
    public int PageIndex { get; set; } = 0;

    public int PageSize { get; set; } = 10;

    public string? SortColumn { get; set; } = "Name";

    public string? SortOrder { get; set; } = "ASC";

    public string? FilterQuery { get; set; } = null;


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> results = new();

        if (PageIndex < 0)
        {
            ValidationResult result = new ValidationResult("Value must be greater than or equal to 0.", new[] { nameof(PageIndex) });
            results.Add(result);
        }
        
        if (PageSize < 1 || PageSize > 100)
        {
            ValidationResult result = new ValidationResult("Value must be between 1 and 100.", new[] { nameof(PageSize) });
            results.Add(result);
        }

        Type entityType = typeof(T);
        if (entityType != null)
        {
            if (!entityType.GetProperties().Any(p => p.Name.ToLower() == SortColumn!.ToLower()))
            {
                ValidationResult result = new ValidationResult("Value must match an existing column.", new[] { nameof(SortColumn) });
                results.Add(result);
            }
        }

        if (SortOrder != "ASC" && SortOrder != "DESC")
        {
            ValidationResult result = new ValidationResult("Value must be one of the following: ASC, DESC.", new[] { nameof(SortOrder) });
            results.Add(result);
        }

        return results;
    }
}
