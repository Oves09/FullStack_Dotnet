using Microsoft.EntityFrameworkCore;
using ProfileBook.API.Data;

namespace ProfileBook.Tests
{
    /// <summary>
    /// Base class for tests that provides common setup for in-memory database
    /// </summary>
    public abstract class TestBase
    {
        protected ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            return new ApplicationDbContext(options);
        }

        protected void SeedTestData(ApplicationDbContext context)
        {
            // Add common test data here if needed
            context.SaveChanges();
        }
    }
}
