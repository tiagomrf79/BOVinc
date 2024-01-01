namespace DummyAPI.DTOs;

public class MilkRecordDto
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public double Milk {  get; set; }
    public double Fat { get; set; }
    public double Protein { get; set; }
    public int SCC { get; set; }
}
