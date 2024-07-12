using JobCandidateHubAPI.DbContext;
using JobCandidateHubAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobCandidateHubAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CandidatesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CandidatesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPut]
    public async Task<IActionResult> UpsertCandidate([FromBody] Candidate candidate)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingCandidate = await _context.Candidates
            .FirstOrDefaultAsync(c => c.Email == candidate.Email);

        if (existingCandidate == null)
        {
            _context.Candidates.Add(candidate);
        }
        else
        {
            existingCandidate.FirstName = candidate.FirstName;
            existingCandidate.LastName = candidate.LastName;
            existingCandidate.PhoneNumber = candidate.PhoneNumber;
            existingCandidate.PreferredCallTime = candidate.PreferredCallTime;
            existingCandidate.LinkedInProfileURL = candidate.LinkedInProfileURL;
            existingCandidate.GitHubProfileURL = candidate.GitHubProfileURL;
            existingCandidate.Comment = candidate.Comment;
        }

        await _context.SaveChangesAsync();

        return Ok(candidate);
    }
}