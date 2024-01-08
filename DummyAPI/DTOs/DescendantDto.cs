namespace DummyAPI.DTOs;

public class DescendantDto
{
    public string DamLabel { get; set; } = string.Empty;
    public string SireLabel {  get; set; } = string.Empty;
    public int? Id { get; set; }
    public int GenderId { get; set; }
    public string DescendantLabel { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
