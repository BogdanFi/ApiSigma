using JobCandidateHubAPI.Controllers;
using JobCandidateHubAPI.Models;
using JobCandidateHubAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace JobCandidateHubAPI.Tests;

public class CandidatesControllerTests
{
    [Fact]
    public async Task UpsertCandidate_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        var candidateServiceMock = new Mock<ICandidateService>();
        var controller = new CandidatesController(candidateServiceMock.Object);
        controller.ModelState.AddModelError("Email", "Required");

        var candidate = new Candidate
        {
            FirstName = "John",
            LastName = "Doe",
            Comment = "Available for full-time opportunities"
        };

        // Act
        var result = await controller.UpsertCandidate(candidate);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task UpsertCandidate_CreatesNewCandidate_WhenCandidateDoesNotExist()
    {
        // Arrange
        var candidateServiceMock = new Mock<ICandidateService>();
        var controller = new CandidatesController(candidateServiceMock.Object);
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
        var result = await controller.UpsertCandidate(candidate);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task UpsertCandidate_UpdatesExistingCandidate_WhenCandidateExists()
    {
        // Arrange
        var candidateServiceMock = new Mock<ICandidateService>();
        var controller = new CandidatesController(candidateServiceMock.Object);
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
        var result = await controller.UpsertCandidate(existingCandidate);
        var result2 = await controller.UpsertCandidate(candidateUpdate);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        var okResult2 = Assert.IsType<OkResult>(result2);
        Assert.Equal(200, okResult2.StatusCode);
    }
}