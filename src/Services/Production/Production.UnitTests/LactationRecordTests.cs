using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Production.API.Infrastructure;

namespace Production.UnitTests
{
    public class LactationRecordTests
    {
        [Fact]
        public async Task Test1()
        {
            var options = new DbContextOptionsBuilder<ProductionContext>().UseSqlServer().Options;
            var context = new ProductionContext(options);
            
        }
    }
}