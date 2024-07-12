using JobCandidateHubAPI.Models;

namespace JobCandidateHubAPI.Repositories.Interfaces;

public interface ICandidateRepository
{
    public Task UpdateAsync(Candidate candidate);
}