using JobCandidateHubAPI.DbContext;
using Microsoft.EntityFrameworkCore;

namespace JobCandidateHubAPI.Tests.Helpers;

public class DatabaseFixture : IDisposable
{
    public ApplicationDbContext Context { get; private set; }

    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        Context = new ApplicationDbContext(options);
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}