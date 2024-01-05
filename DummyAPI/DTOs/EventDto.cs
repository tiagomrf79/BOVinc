namespace DummyAPI.DTOs;

public class EventDto
{
    public int Id {  get; set; }
    public DateOnly Date { get; set; }
    public int EventTypeId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public int? ReasonForEnteringId { get; set; }
    public string? ReasonForEntering { get; set; }
    public int? ReasonForLeavingId { get; set; }
    public string? ReasonForLeaving { get; set; }
    public int? BreedingBullId { get; set; }
    public bool? IsCatalogSire {  get; set; }
    public string? BreedingBull { get; set; }
    public int? PreviousGroupId { get; set; }
    public string? PreviousGroup {  get; set; }
    public int? NewGroupId { get; set; }
    public string? NewGroup { get; set; }
    public string? EventDescription { get; set; }
    public string? Notes { get; set; }
}
