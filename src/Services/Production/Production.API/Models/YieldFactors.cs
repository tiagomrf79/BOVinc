namespace Production.API.Models;

public class FirstTestFactor
{
    public int DayOfFirstSampleMin { get; set; }
    public int DayOfFirstSampleMax { get; set; }

    public bool IsFirstlactation { get; set; }

    public double MilkFactor { get; set; }
    public double FatFactor { get; set; }
    public double ProteinFactor { get; set; }
}

public class PeakTestFactor
{
    public int DayOfPreviousSampleMin { get; set; }
    public int DayOfPreviousSampleMax { get; set; }

    public int TestIntervalMin { get; set; }
    public int TestIntervalMax { get; set; }

    public bool IsFirstlactation { get; set; }

    public double MilkFactor { get; set; }
    public double FatFactor { get; set; }
    public double ProteinFactor { get; set; }
}

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
