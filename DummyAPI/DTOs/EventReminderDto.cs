namespace DummyAPI.DTOs;

public class EventReminderDto
{
    //reminder and event
    public bool IsReminder { get; set; } = false;
    public DateOnly Date { get; set; }


    //reminder only
    public int? ReminderTypeId { get; set; } //1-heat,2-dry,3-calving,4-confirmation,...
    public string? ReminderType { get; set; }

    
    //event only
    public int? Id {  get; set; }
    public int? EventTypeId { get; set; }
    public string? EventType { get; set; } = string.Empty;
    public int? ReasonForEnteringId { get; set; }
    public string? ReasonForEntering { get; set; }
    public int? ReasonForLeavingId { get; set; }
    public string? ReasonForLeaving { get; set; }
    public bool? IsBreedingBullKnown { get; set; }
    public bool? IsCatalogSire { get; set; }
    public int? BreedingBullId { get; set; }
    public string? BreedingBull { get; set; }
    public int? PreviousGroupId { get; set; }
    public string? PreviousGroup {  get; set; }
    public int? NewGroupId { get; set; }
    public string? NewGroup { get; set; }
    public string? EventDescription { get; set; }
    public string? Notes { get; set; }
}
