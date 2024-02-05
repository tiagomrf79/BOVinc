using Microsoft.EntityFrameworkCore;
using Production.API.Models;

namespace Production.API.Infrastructure.Repositories;

public class TestSampleRepository : ITestSampleRepository
{
    private readonly ProductionContext _context;

    public TestSampleRepository(ProductionContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<TestSample?> GetTestSampleByIdAsync(int id)
    {
         return await _context.TestSamples.FindAsync(id);
    }

    public async Task<IEnumerable<TestSample>> GetSortedTestSamplesForPeriodAsync(int animalId, DateOnly periodStart, DateOnly? periodEnd)
    {
        var list = new List<TestSample>();

        list = await _context.TestSamples
            .Where(x => 
                x.AnimalId == animalId
                && x.Date >= periodStart
                && (periodEnd == null || x.Date <= periodEnd))
            .OrderBy(x => x.Date)
            .ToListAsync();

        return list;
    }

    public async Task<bool> IsTestSampleDuplicatedAsync(int animalId, DateOnly date, double milkYield, int? id = null)
    {
        bool exists = false;
        
        exists = await _context.TestSamples
            .Where(x =>
                x.AnimalId == animalId
                && x.Date == date
                && x.MilkYield == milkYield
                && (id == null || x.Id != id))
            .AnyAsync();

        return exists;
    }

    public async Task CreateTestSampleAsync(TestSample testSample)
    {
        testSample.CreatedAt = DateTime.UtcNow;
        testSample.LastUpdatedAt = testSample.CreatedAt;
        await _context.TestSamples.AddAsync(testSample);
    }

    public void UpdateTestSample(TestSample testSample)
    {
        testSample.LastUpdatedAt = DateTime.UtcNow;
        _context.TestSamples.Update(testSample);
    }

    public void DeleteTestSample(TestSample testSample)
    {
        _context.TestSamples.Remove(testSample);
    }

    public async Task CommitChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
