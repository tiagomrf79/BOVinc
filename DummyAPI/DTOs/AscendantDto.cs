namespace DummyAPI.DTOs;

public class AscendantDto
{
    public string AnimalLabel { get; set; } = string.Empty;
    public IEnumerable<ParentDto> Parents { get; set; } = Enumerable.Empty<ParentDto>();
}

public class ParentDto
{
    public int Id { get; set; }
    public int GenderId { get; set; }
    public string ParentLabel { get; set; } = string.Empty;
    public string? ParentScore { get; set; }
    public IEnumerable<GrandParentDto> GrandParents { get; set; } = Enumerable.Empty<GrandParentDto>();
}

public class GrandParentDto
{
    public int Id { get; set; }
    public int GenderId { get; set; }
    public string GrandParentLabel { get; set; } = string.Empty;
    public string? GrandParentScore { get; set; }

}