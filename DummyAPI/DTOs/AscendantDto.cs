namespace DummyAPI.DTOs;

public class AscendantDto
{
    public int Id { get; set; }
    public string AscendantLabel { get; set; } = string.Empty;
    public string? FeatureScore { get; set; }
    public AscendantDto? Dam {  get; set; }
    public AscendantDto? Sire {  get; set; }
}

