namespace DummyAPI.DTOs;

public class ParentOptionForDropdownDto
{
    public int Id { get; set; }
    public string? RegistrationId { get; set; }
    public string? Name { get; set; }
    public string Source { get; set; } = string.Empty;
}
