using Animal.API.Exceptions;

namespace Animal.API.Enums;

public class Category : Enumeration
{
    public static Category Calf = new Category(1, nameof(Calf));
    public static Category Heifer = new Category(2, nameof(Heifer));
    public static Category MilkingCow = new Category(3, "Milking Cow");
    public static Category DryCow = new Category(4, "Dry Cow");
    public static Category Bull = new Category(5, nameof(Bull));
    public static Category Steer = new Category(6, nameof(Steer));

    public Category(int id, string name)
        : base(id, name)
    { }

    public static IEnumerable<Category> List()
        => new[] { Calf, Heifer, MilkingCow, DryCow, Bull, Steer };

    public static Category FromName(string name)
    {
        var state = List().SingleOrDefault(s =>
                        string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

        if (state == null)
        {
            throw new AnimalException($"Possible values for Category: {string.Join(",", List().Select(s => s.Name))}");
        }

        return state;
    }

    public static Category From(int id)
    {
        var state = List().SingleOrDefault(s => s.Id == id);

        if (state == null)
        {
            throw new AnimalException($"Possible values for Category: {string.Join(",", List().Select(s => s.Name))}");
        }

        return state;
    }
}
