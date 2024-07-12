using JobCandidateHubAPI.Controllers;
using JobCandidateHubAPI.DbContext;
using JobCandidateHubAPI.Models;
using JobCandidateHubAPI.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace JobCandidateHubAPI.Tests;

public class CandidatesControllerTests : IClassFixture<DatabaseFixture>
{
    private readonly ApplicationDbContext _context;
    private readonly CandidatesController _controller;

    public CandidatesControllerTests(DatabaseFixture fixture)
    {
        _context = fixture.Context;
        _controller = new CandidatesController(_context);
    }

    private void CleanDatabase()
    {
        _context.Candidates.RemoveRange(_context.Candidates);
        _context.SaveChanges();
    }

    [Fact]
    public async Task UpsertCandidate_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        CleanDatabase();
        _controller.ModelState.AddModelError("Email", "Required");

        var candidate = new Candidate
        {
            FirstName = "John",
            LastName = "Doe",
            Comment = "Available for full-time opportunities"
        };

        // Act
        var result = await _controller.UpsertCandidate(candidate);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task UpsertCandidate_CreatesNewCandidate_WhenCandidateDoesNotExist()
    {
        // Arrange
        CleanDatabase();
        var candidate = new Candidate
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Comment = "Available for full-time opportunities",
            GitHubProfileURL = "https://github.com/johndoe",
            LinkedInProfileURL = "https://www.linkedin.com/in/johndoe",
            PhoneNumber = "123-456-7890",
            PreferredCallTime = DateTime.Now.AddHours(1)
        };

        // Act
        var result = await _controller.UpsertCandidate(candidate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Single(_context.Candidates.ToList());
        Assert.Equal(candidate.Email, _context.Candidates.First().Email);
    }

    [Fact]
    public async Task UpsertCandidate_UpdatesExistingCandidate_WhenCandidateExists()
    {
        // Arrange
        CleanDatabase();
        var existingCandidate = new Candidate
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane.doe@example.com",
            Comment = "Looking for new opportunities",
            GitHubProfileURL = "https://github.com/janedoe",
            LinkedInProfileURL = "https://www.linkedin.com/in/janedoe",
            PhoneNumber = "098-765-4321",
            PreferredCallTime = DateTime.Now.AddHours(2)
        };

        _context.Candidates.Add(existingCandidate);
        await _context.SaveChangesAsync();

        var candidateUpdate = new Candidate
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.doe@example.com",
            Comment = "Updated comment",
            GitHubProfileURL = "https://github.com/janesmith",
            LinkedInProfileURL = "https://www.linkedin.com/in/janesmith",
            PhoneNumber = "111-222-3333",
            PreferredCallTime = DateTime.Now.AddHours(3)
        };

        // Act
        var result = await _controller.UpsertCandidate(candidateUpdate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Single(_context.Candidates.ToList());
        var updatedCandidate = _context.Candidates.First();
        Assert.Equal("Smith", updatedCandidate.LastName);
        Assert.Equal("Updated comment", updatedCandidate.Comment);
    }
}