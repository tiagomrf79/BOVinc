namespace DummyAPI.DTOs;

public class DescendantDto
{
    public string ParentsCross { get; set; } = string.Empty;
    public int Id { get; set; }
    public int GenderId { get; set; }
    public string AnimalName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
