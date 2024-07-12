using JobCandidateHubAPI.Models;
using JobCandidateHubAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobCandidateHubAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CandidatesController(ICandidateService candidateService) : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> UpsertCandidate([FromBody] Candidate candidate)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await candidateService.UpdateCandidateAsync(candidate);

        return Ok();
    }
}