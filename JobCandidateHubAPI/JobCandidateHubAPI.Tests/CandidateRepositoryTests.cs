using JobCandidateHubAPI.DbContext;
using JobCandidateHubAPI.Models;
using JobCandidateHubAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JobCandidateHubAPI.Tests;

public class CandidateRepositoryTests
{
    [Fact]
    public async Task UpdateAsync_ShouldComplete_UpdatesExistingCandidate()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        var candidateToUpdate = new Candidate
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "123-456-7890",
            LinkedInProfileURL = "https://linkedin.com/in/johndoe",
            GitHubProfileURL = "https://github.com/johndoe",
            PreferredCallTime = DateTime.Now.AddHours(1),
            Comment = "Available for full-time opportunities"
        };

        await using (var dbContext = new ApplicationDbContext(options))
        {
            dbContext.Candidates.Add(candidateToUpdate);
            await dbContext.SaveChangesAsync();
        }

        var updatedCandidate = new Candidate
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "987-654-3210", // Updated phone number
            LinkedInProfileURL = "https://linkedin.com/in/johndoe",
            GitHubProfileURL = "https://github.com/johndoe",
            PreferredCallTime = DateTime.Now.AddHours(2),
            Comment = "Still available"
        };

        // Act
        await using (var dbContext = new ApplicationDbContext(options))
        {
            var repository = new CandidateRepository(dbContext);
            await repository.UpdateAsync(updatedCandidate);
        }

        // Assert
        await using (var dbContext = new ApplicationDbContext(options))
        {
            var repository = new CandidateRepository(dbContext);
            var candidate = dbContext.Candidates.FirstOrDefault(c => c.Email == updatedCandidate.Email);

            Assert.NotNull(candidate);
            Assert.Equal(updatedCandidate.PhoneNumber, candidate.PhoneNumber);
            Assert.Equal(updatedCandidate.PreferredCallTime, candidate.PreferredCallTime);
            Assert.Equal(updatedCandidate.Comment, candidate.Comment);
        }
    }
}