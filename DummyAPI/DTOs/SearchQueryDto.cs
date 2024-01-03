using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DummyAPI.DTOs;

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

    /// <summary>Search string</summary>
    [DefaultValue(null)]
    public string? FilterQuery { get; set; } = null;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> results = new();
        return results;
    }
}