namespace Production.API.Models;

public class Lactation
{
    public int Id { get; set; }
    public int LactationNumber {  get; set; }
    public DateOnly CalvingDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int AnimalId { get; set; }

    //  Any milk measurements before the earliest lactation CalvingDate will be ignored
    //  Any milk measurements between current CalvingDate and previous EndDate will be ignored
    //  Any milk measurements between current EndDate and next CalvingDate will be ignored
    //  Deleting this lactation can ignore the measurements between its CalvingDate and its EndDate
    //  Changing a lactations CalvingDate and/or EndDate can ignore the measurements that occurred between previous and current dates
}
