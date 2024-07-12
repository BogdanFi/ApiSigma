using JobCandidateHubAPI.Models;

namespace JobCandidateHubAPI.Services.Interfaces;

public interface ICandidateService
{
    public Task UpdateCandidateAsync(Candidate candidate);
}