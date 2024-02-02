namespace Production.API.Models;

public class LastTestFactor
{
    public int DayOfLastSampleMin { get; set; }
    public int DayOfLastSampleMax { get; set; }

    public int TestIntervalMin { get; set; }
    public int TestIntervalMax { get; set; }

    public bool IsFirstlactation { get; set; }

    public double MilkFactor { get; set; }
    public double FatFactor { get; set; }
    public double ProteinFactor { get; set; }
}
