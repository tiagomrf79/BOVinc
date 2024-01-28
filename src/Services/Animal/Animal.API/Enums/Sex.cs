using Animal.API.Exceptions;

namespace Animal.API.Enums;

public class Sex : Enumeration
{
    public static Sex Female = new Sex(1, nameof(Female));
    public static Sex Male = new Sex(2, nameof(Male));


    public Sex(int id, string name)
        : base(id, name)
    { }

    public static IEnumerable<Sex> List()
        => new[] { Female, Male };

    public static Sex FromName(string name)
    {
        var state = List().SingleOrDefault(s =>
                        string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

        if (state == null)
        {
            throw new AnimalException($"Possible values for Sex: {string.Join(",", List().Select(s => s.Name))}");
        }

        return state;
    }

    public static Sex From(int id)
    {
        var state = List().SingleOrDefault(s => s.Id == id);

        if (state == null)
        {
            throw new AnimalException($"Possible values for Sex: {string.Join(",", List().Select(s => s.Name))}");
        }

        return state;
    }
}
