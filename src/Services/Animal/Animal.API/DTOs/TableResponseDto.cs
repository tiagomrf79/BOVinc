namespace Animal.API.DTOs;

public class TableResponseDto<T>
{
    public int TotalCount { get; set; }
    public IEnumerable<T> Rows { get; set; } = Enumerable.Empty<T>();
}
