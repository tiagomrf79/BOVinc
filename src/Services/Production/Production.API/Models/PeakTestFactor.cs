namespace Production.API.Models;

public class PeakTestFactor
{
    public int DayOfFirstSampleMin { get; set; }
    public int DayOfFirstSampleMax { get; set; }

    public int TestIntervalMin { get; set; }
    public int TestIntervalMax { get; set; }

    public bool IsFirstlactationCow { get; set; }

    public double MilkFactor { get; set; }
    public double FatFactor { get; set; }
    public double ProteinFactor { get; set; }
}
