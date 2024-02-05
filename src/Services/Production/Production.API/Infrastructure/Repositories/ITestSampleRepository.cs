using Production.API.Models;

namespace Production.API.Infrastructure.Repositories;

public interface ITestSampleRepository
{
    Task<TestSample?> GetTestSampleByIdAsync(int id);
    Task<IEnumerable<TestSample>> GetSortedTestSamplesForPeriodAsync(int animalId, DateOnly periodStart, DateOnly? periodEnd);
    Task<bool> IsTestSampleDuplicatedAsync(int animalId, DateOnly date, double milkYield, int? id = null);
    Task CreateTestSampleAsync(TestSample testSample);
    void UpdateTestSample(TestSample testSample);
    void DeleteTestSample(TestSample testSample);
    Task CommitChangesAsync();
}
