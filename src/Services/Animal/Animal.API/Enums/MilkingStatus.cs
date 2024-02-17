using Animal.API.Exceptions;
using Animal.API.Models;

namespace Animal.API.Enums;

public class MilkingStatus : Enumeration
{
    public static MilkingStatus Milking = new MilkingStatus(1, nameof(Milking));
    public static MilkingStatus Dry = new MilkingStatus (2, nameof(Dry));
    
    public MilkingStatus(int id, string name)
        : base(id, name)
    { }

    public static IEnumerable<MilkingStatus> List()
        => new[] { Milking, Dry };


    public static MilkingStatus FromName(string name)
    {
        var state = List().SingleOrDefault(s =>
                        string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

        if (state == null)
        {
            throw new AnimalException($"Possible values for MilkingStatus: {string.Join(",", List().Select(s => s.Name))}");
        }

        return state;
    }

    public static MilkingStatus From(int id)
    {
        var state = List().SingleOrDefault(s => s.Id == id);

        if (state == null)
        {
            throw new AnimalException($"Possible values for MilkingStatus: {string.Join(",", List().Select(s => s.Name))}");
        }

        return state;
    }
}
