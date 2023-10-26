using AnimalsAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace AnimalsAPI.DTOs;

/// <summary>
/// A DTO object containing the data to create a new animal.
/// </summary>
public class AnimalUpdateDto
{
    /// <summary>Farm ID</summary>
    /// <example>5</example>
    [Required]
    public int FarmId { get; set; }

    /// <summary>Registered identification</summary>
    /// <example>PT393224973</example>
    [MaxLength(25)]
    public string? RegistrationId { get; set; }

    /// <summary>Name the animal responds to</summary>
    /// <example>Marquesa</example>
    [MaxLength(25)]
    public string? Name { get; set; }

    /// <summary>Farm identification tag</summary>
    /// <example>4973</example>
    [MaxLength(10)]
    public string? Tag { get; set; }

    //add enums

    /// <summary>Date of birth</summary>
    /// <example>14/10/2018</example>
    [Required]
    public DateOnly? DataOfBirth { get; set; }

    /// <summary>ID of the mother if known</summary>
    /// <example>PT193162686</example>
    public int? MotherId { get; set; }

    /// <summary>ID of the father if known</summary>
    /// <example>PT193198478</example>
    public int? FatherId { get; set; }
}
