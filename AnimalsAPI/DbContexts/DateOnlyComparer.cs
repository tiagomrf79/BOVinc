﻿using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AnimalsAPI.DbContexts;

public class DateOnlyComparer : ValueComparer<DateOnly>
{
    public DateOnlyComparer() : base(
        (x, y) => x.DayNumber == y.DayNumber,
        dateOnly => dateOnly.GetHashCode())
    {
    }
}
