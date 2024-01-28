namespace Animal.API.Models;

public class Breed
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int GestationLength { get; set; }
}
