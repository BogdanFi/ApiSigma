using System.ComponentModel.DataAnnotations;

namespace JobCandidateHubAPI.Models;

public class Candidate
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    public string PhoneNumber { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public DateTime PreferredCallTime { get; set; }

    [Url]
    public string LinkedInProfileURL { get; set; }

    [Url]
    public string GitHubProfileURL { get; set; }

    [Required]
    public string Comment { get; set; }
}