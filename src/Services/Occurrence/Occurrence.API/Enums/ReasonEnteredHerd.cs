using Occurrence.API.Exceptions;

namespace Occurrence.API.Enums;

public class ReasonEnteredHerd : Enumeration
{
    public static ReasonEnteredHerd BornInFarm = new ReasonEnteredHerd(1, "Born in farm");
    public static ReasonEnteredHerd InitialInventory = new ReasonEnteredHerd(2, "Initial inventory");
    public static ReasonEnteredHerd Bought = new ReasonEnteredHerd(3, nameof(Bought));

    public ReasonEnteredHerd(int id, string name)
        : base(id, name)
    { }

    public static IEnumerable<ReasonEnteredHerd> List()
        => new[] { BornInFarm, InitialInventory, Bought };

    public static ReasonEnteredHerd FromName(string name)
    {
        var state = List().SingleOrDefault(s =>
                        string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

        if (state == null)
        {
            throw new OccurrenceException($"Possible values for ReasonEnteredHerd: {string.Join(",", List().Select(s => s.Name))}");
        }

        return state;
    }

    public static ReasonEnteredHerd From(int id)
    {
        var state = List().SingleOrDefault(s => s.Id == id);

        if (state == null)
        {
            throw new OccurrenceException($"Possible values for ReasonEnteredHerd: {string.Join(",", List().Select(s => s.Name))}");
        }

        return state;
    }
}
