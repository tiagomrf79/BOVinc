using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FarmsAPI.DTO;

/// <summary>
/// A DTO object containing the data to create a new farm.
/// </summary>
public class FarmUpdateDto
{
    /// <summary>Farm name</summary>
    /// <example>Demo farm</example>
    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }

    /// <summary>Address</summary>
    /// <example>Rua Doutor Malheiros 18</example>
    [MaxLength(200)]
    public string? Address { get; set; }

    /// <summary>City</summary>
    /// <example>Sintra</example>
    [MaxLength(50)]
    public string? City { get; set; }

    /// <summary>Region</summary>
    /// <example>Lisbon</example>
    [MaxLength(50)]
    public string? Region { get; set; }

    /// <summary>Country</summary>
    /// <example>Portugal</example>
    [MaxLength(50)]
    public string? Country { get; set; }
}
