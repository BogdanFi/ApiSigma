using JobCandidateHubAPI.Models;
using JobCandidateHubAPI.Repositories.Interfaces;
using JobCandidateHubAPI.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace JobCandidateHubAPI.Services;

public class CandidateService(ICandidateRepository candidateRepository, IMemoryCache cache)
    : ICandidateService
{
    public async Task UpdateCandidateAsync(Candidate candidate)
    {
        if (cache.TryGetValue(candidate.Email, out Candidate cachedCandidate))
        {
            if (AreCandidatesEqual(candidate, cachedCandidate))
            {
                return;
            }
        }
        // Update candidate in repository
        await candidateRepository.UpdateAsync(candidate);

        // Add this candidate in cache
        cache.Set(candidate.Email, candidate, TimeSpan.FromMinutes(15));
    }

    private bool AreCandidatesEqual(Candidate candidate1, Candidate candidate2)
    {
        if (candidate1 == null || candidate2 == null)
        {
            return false;
        }

        return candidate1.FirstName == candidate2.FirstName &&
               candidate1.LastName == candidate2.LastName &&
               candidate1.Email == candidate2.Email &&
               candidate1.PhoneNumber == candidate2.PhoneNumber &&
               candidate1.PreferredCallTime == candidate2.PreferredCallTime &&
               candidate1.LinkedInProfileURL == candidate2.LinkedInProfileURL &&
               candidate1.GitHubProfileURL == candidate2.GitHubProfileURL &&
               candidate1.Comment == candidate2.Comment;
    }
}