using Occurrence.API.Exceptions;

namespace Occurrence.API.Enums;

public class ReasonLeftHerd : Enumeration
{
    public static ReasonLeftHerd Died = new ReasonLeftHerd(1, nameof(Died));
    public static ReasonLeftHerd Sold = new ReasonLeftHerd(2, nameof(Sold));
    public static ReasonLeftHerd Culled = new ReasonLeftHerd(3, nameof(Culled));

    public ReasonLeftHerd(int id, string name) 
        : base(id, name)
    { }

    public static IEnumerable<ReasonLeftHerd> List()
        => new[] { Died, Sold, Culled };

    public static ReasonLeftHerd FromName(string name)
    {
        var state = List().SingleOrDefault(s =>
                        string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

        if (state == null)
        {
            throw new OccurrenceException($"Possible values for ReasonLeftHerd: {string.Join(",", List().Select(s => s.Name))}");
        }

        return state;
    }

    public static ReasonLeftHerd From(int id)
    {
        var state = List().SingleOrDefault(s => s.Id == id);

        if (state == null)
        {
            throw new OccurrenceException($"Possible values for ReasonLeftHerd: {string.Join(",", List().Select(s => s.Name))}");
        }

        return state;
    }
}
