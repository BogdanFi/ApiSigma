using JobCandidateHubAPI.Models;
using JobCandidateHubAPI.Repositories.Interfaces;
using JobCandidateHubAPI.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace JobCandidateHubAPI.Tests;

public class CandidateServiceTests
{
    [Fact]
    public async Task UpdateCandidateAsync_DoesNotUpdate_WhenCandidateIsCachedAndEqual()
    {
        // Arrange
        var candidate = new Candidate
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "123-456-7890",
            PreferredCallTime = DateTime.Now.AddHours(1),
            LinkedInProfileURL = "https://linkedin.com/in/johndoe",
            GitHubProfileURL = "https://github.com/johndoe",
            Comment = "Available"
        };

        var mockRepository = new Mock<ICandidateRepository>();
        var mockCache = new Mock<IMemoryCache>();

        object cacheCandidate = candidate;
        mockCache.Setup(c => c.TryGetValue(candidate.Email, out cacheCandidate))
                 .Returns(true);

        var service = new CandidateService(mockRepository.Object, mockCache.Object);

        // Act
        await service.UpdateCandidateAsync(candidate);

        // Assert
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Candidate>()), Times.Never);
    }

    [Fact]
    public async Task UpdateCandidateAsync_UpdatesCandidate_WhenCandidateIsNotCached()
    {
        // Arrange
        var candidate = new Candidate
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "123-456-7890",
            PreferredCallTime = DateTime.Now.AddHours(1),
            LinkedInProfileURL = "https://linkedin.com/in/johndoe",
            GitHubProfileURL = "https://github.com/johndoe",
            Comment = "Available"
        };

        var mockRepository = new Mock<ICandidateRepository>();
        var mockCache = new Mock<IMemoryCache>();

        object cacheCandidate;
        mockCache.Setup(c => c.TryGetValue(candidate.Email, out cacheCandidate))
                 .Returns(false);

        var cacheEntry = new Mock<ICacheEntry>();
        mockCache.Setup(c => c.CreateEntry(It.IsAny<object>())).Returns(cacheEntry.Object);

        var service = new CandidateService(mockRepository.Object, mockCache.Object);

        // Act
        await service.UpdateCandidateAsync(candidate);

        // Assert
        mockRepository.Verify(r => r.UpdateAsync(It.Is<Candidate>(c => c.Email == candidate.Email)), Times.Once);
    }

    [Fact]
    public async Task UpdateCandidateAsync_UpdatesCandidate_WhenCandidateIsCachedButDifferent()
    {
        // Arrange
        var existingCandidate = new Candidate
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "123-456-7890",
            PreferredCallTime = DateTime.Now.AddHours(1),
            LinkedInProfileURL = "https://linkedin.com/in/johndoe",
            GitHubProfileURL = "https://github.com/johndoe",
            Comment = "Available"
        };

        var updatedCandidate = new Candidate
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "987-654-3210", // Updated phone number
            PreferredCallTime = DateTime.Now.AddHours(1),
            LinkedInProfileURL = "https://linkedin.com/in/johndoe",
            GitHubProfileURL = "https://github.com/johndoe",
            Comment = "Available"
        };

        var mockRepository = new Mock<ICandidateRepository>();
        var mockCache = new Mock<IMemoryCache>();

        object cacheCandidate = existingCandidate;
        mockCache.Setup(c => c.TryGetValue(existingCandidate.Email, out cacheCandidate))
                 .Returns(true);

        var cacheEntry = new Mock<ICacheEntry>();
        mockCache.Setup(c => c.CreateEntry(It.IsAny<object>())).Returns(cacheEntry.Object);

        var service = new CandidateService(mockRepository.Object, mockCache.Object);

        // Act
        await service.UpdateCandidateAsync(updatedCandidate);

        // Assert
        mockRepository.Verify(r => r.UpdateAsync(It.Is<Candidate>(c => c.Email == updatedCandidate.Email)), Times.Once);
    }
}