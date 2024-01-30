namespace Production.API.Models;

public class MilkRecord
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public double? MilkYield { get; set; }
    public double? FatPercentage { get; set; }
    public double? ProteinPercentage {  get; set; }
    public int? SomaticCellCount { get; set; }
    public int AnimalId { get; set; }

    //Milk measurements are assigned to the given lactation at runtime

    //don't allow duplicate measurements (same date, milk, fat, protein, SCC and AnimalId)
    //Measurement date must be after a CalvingDate and
    //  before an EndDate if not last lactation
    //  or before the next CalvingDate if not last lactation and EndDate is null
    //Measurement date must be 
    //AnimalId must belong to a cow with at least one lactation
    //Measurement date cannot be before the cows earliest lactation CalvingDate
    //Measurement date cannot be after today
    //MilkYield, FatPercentage, ProteinPercentage and SomaticCellCount must be null or greater than or equal to 0
    //At least one of MilkYield, FatPercentage, ProteinPercentage and SomaticCellCount must not be null
}
