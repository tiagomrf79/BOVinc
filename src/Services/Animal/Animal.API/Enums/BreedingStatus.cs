using Animal.API.Exceptions;

namespace Animal.API.Enums;

public class BreedingStatus : Enumeration
{
    public static BreedingStatus Open = new BreedingStatus(1, nameof(Open));
    public static BreedingStatus Bred = new BreedingStatus(1, nameof(Bred));
    public static BreedingStatus Confirmed = new BreedingStatus(1, nameof(Confirmed));

    public BreedingStatus(int id, string name)
        : base(id, name)
    { }

    public static IEnumerable<BreedingStatus> List()
        => new[] { Open, Bred, Confirmed };


    public static BreedingStatus FromName(string name)
    {
        var state = List().SingleOrDefault(s =>
                        string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

        if (state == null)
        {
            throw new AnimalException($"Possible values for BreedingStatus: {string.Join(",", List().Select(s => s.Name))}");
        }

        return state;
    }

    public static BreedingStatus From(int id)
    {
        var state = List().SingleOrDefault(s => s.Id == id);

        if (state == null)
        {
            throw new AnimalException($"Possible values for BreedingStatus: {string.Join(",", List().Select(s => s.Name))}");
        }

        return state;
    }
}
