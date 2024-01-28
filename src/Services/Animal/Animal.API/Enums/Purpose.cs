using Animal.API.Exceptions;

namespace Animal.API.Enums;

public class Purpose : Enumeration
{
    public static Purpose Breeding = new Purpose(1, nameof(Breeding));
    public static Purpose Milk = new Purpose(1, nameof(Milk));
    public static Purpose Meat = new Purpose(1, nameof(Meat));
    public static Purpose ToCull = new Purpose(1, "To cull");
    public static Purpose ToSell = new Purpose(1, "To sell");

    public Purpose(int id, string name)
        : base(id, name)
    { }

    public static IEnumerable<Purpose> List()
        => new[] { Breeding, Milk, Meat, ToCull, ToSell };


    public static Purpose FromName(string name)
    {
        var state = List().SingleOrDefault(s =>
                        string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

        if (state == null)
        {
            throw new AnimalException($"Possible values for Purpose: {string.Join(",", List().Select(s => s.Name))}");
        }

        return state;
    }

    public static Purpose From(int id)
    {
        var state = List().SingleOrDefault(s => s.Id == id);

        if (state == null)
        {
            throw new AnimalException($"Possible values for Purpose: {string.Join(",", List().Select(s => s.Name))}");
        }

        return state;
    }
}
