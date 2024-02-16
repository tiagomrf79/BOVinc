using Occurrence.API.Exceptions;

namespace Occurrence.API.Enums;

public class EventType : Enumeration
{
    public static EventType Born = new EventType(1, nameof(Born));
    public static EventType EnteredHerd = new EventType(2, "Entered herd");
    public static EventType Prostaglandin = new EventType(3, nameof(Prostaglandin));
    public static EventType Heat = new EventType(4, nameof(Heat));
    public static EventType Breeding = new EventType(5, nameof(Breeding));
    public static EventType NegativePregnancy = new EventType(6, "Negative pregnancy");
    public static EventType ConfirmedPregnant = new EventType(7, "Confirmed pregnant");
    public static EventType DryOff = new EventType(8, "Dry off");
    public static EventType Abortion = new EventType(9, nameof(Abortion));
    public static EventType AbortionNewLactation = new EventType(10, "Abortion (new lactation)");
    public static EventType Calving = new EventType(11, nameof(Calving));
    public static EventType Weaning = new EventType(12, nameof(Weaning));
    public static EventType Vaccination = new EventType(13, nameof(Vaccination));
    public static EventType Diagnosis = new EventType(14, nameof(Diagnosis));
    public static EventType ChangedGroup = new EventType(15, "Changed group");
    public static EventType LeftHerd = new EventType(16, "Left herd");
    public static EventType Died = new EventType(176, nameof(Died));

    public EventType(int id, string name) : base(id, name)
    { }

    public static IEnumerable<EventType> List()
        => new[] { Born, EnteredHerd, Prostaglandin, Heat, Breeding, NegativePregnancy, ConfirmedPregnant, DryOff,
        Abortion, AbortionNewLactation, Calving, Weaning, Vaccination, Diagnosis, ChangedGroup, LeftHerd, Died};

    public static EventType FromName(string name)
    {
        var state = List().SingleOrDefault(s =>
                        string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

        if (state == null)
        {
            throw new OccurrenceException($"Possible values for EventType: {string.Join(",", List().Select(s => s.Name))}");
        }

        return state;
    }

    public static EventType From(int id)
    {
        var state = List().SingleOrDefault(s => s.Id == id);

        if (state == null)
        {
            throw new OccurrenceException($"Possible values for EventType: {string.Join(",", List().Select(s => s.Name))}");
        }

        return state;
    }
}
