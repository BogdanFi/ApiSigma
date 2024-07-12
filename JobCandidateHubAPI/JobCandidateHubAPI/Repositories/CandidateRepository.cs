using JobCandidateHubAPI.DbContext;
using JobCandidateHubAPI.Models;
using JobCandidateHubAPI.Repositories.Interfaces;

namespace JobCandidateHubAPI.Repositories;

public class CandidateRepository(ApplicationDbContext dbContext) : ICandidateRepository
{
    public async Task UpdateAsync(Candidate candidate)
    {
        dbContext.Candidates.Update(candidate);
        await dbContext.SaveChangesAsync();
    }
}