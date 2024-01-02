﻿namespace DummyAPI.DTOs;

public class MilkRecordForChartDto
{
    public DateOnly CalvingDate { get; set; }
    public IEnumerable<MilkOnlyRecordDto> ActualMilkRecords { get; set; } = Enumerable.Empty<MilkOnlyRecordDto>();
    public IEnumerable<MilkOnlyRecordDto> PredictedMilkRecords { get; set; } = Enumerable.Empty<MilkOnlyRecordDto>();
}
