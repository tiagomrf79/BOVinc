using AnimalsAPI.Models;

namespace AnimalsAPI.DTOs;

public class AnimalResponseDto
{
    public int Id { get; set; }
    public int? FarmId { get; set; }
    public string? RegistrationId { get; set; }
    public string? Name { get; set; }
    public string? Tag { get; set; }
    
    //add enums to return

    public DateOnly? DataOfBirth { get; set; }
    public int? MotherId { get; set; }
    public int? FatherId { get; set; }

}
