using Animal.API.Exceptions;

namespace Animal.API.Enums;

public class Catalog : Enumeration
{
    public static Catalog InitialInventory = new Catalog(1, "Initial inventory");
    public static Catalog HistoricRecord = new Catalog(2, "Historic record");
    public static Catalog Calving = new Catalog(3, nameof(Calving));
    public static Catalog Transfer = new Catalog(4, nameof(Transfer));

    public Catalog(int id, string name)
        : base(id, name)
    { }

    public static IEnumerable<Catalog> List()
        => new[] { InitialInventory, HistoricRecord, Calving, Transfer };

    public static Catalog FromName(string name)
    {
        var state = List().SingleOrDefault(s =>
                        string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

        if (state == null)
        {
            throw new AnimalException($"Possible values for Catalog: {string.Join(",", List().Select(s => s.Name))}");
        }

        return state;
    }

    public static Catalog From(int id)
    {
        var state = List().SingleOrDefault(s => s.Id == id);

        if (state == null)
        {
            throw new AnimalException($"Possible values for Catalog: {string.Join(",", List().Select(s => s.Name))}");
        }

        return state;
    }
}
