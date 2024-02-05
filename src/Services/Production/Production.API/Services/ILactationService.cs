namespace Production.API.Services;

public interface ILactationService
{
    Task<Tuple<int, int, int>> GetTotalYield(
        List<double> milkYields,
        List<double?> fatYields,
        List<double?> proteinYields,
        List<int> daysInMilk,
        int lactationNumber);
    Task<Tuple<int, int, int>> GetTotalYieldStandardized(
        List<double> milkYields,
        List<double?> fatYields,
        List<double?> proteinYields,
        List<int> daysInMilk,
        int lactationNumber);

    Task<List<Tuple<DateOnly, double, bool>>> GetLactationCurve(
        List<double> milkYields,
        List<int> daysInMilk,
        int lactationNumber);
}
