using ContractorCompliance.Models;
using Microsoft.AspNetCore.Mvc;
namespace ContractorCompliance.Controllers;
[ApiController]
[Route("api/credentials")]
public class CredentialsController : ControllerBase
{
    private static readonly List<Credential> Records = [];
    private static readonly List<object> Audits = [];
    [HttpGet] public IActionResult List() => Ok(Records);
    [HttpPost] public IActionResult Submit(CredentialInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Contractor) || string.IsNullOrWhiteSpace(input.CredentialType)) return BadRequest(new { error = "Contractor and credential type are required" });
        if (Request.Headers["X-Role"] != "credential-clerk") return StatusCode(StatusCodes.Status403Forbidden, new { error = "Credential clerk role is required" });
        var record = new Credential(Records.Count + 1, input.Contractor, input.CredentialType, input.ExpiresOn, "pending"); Records.Add(record); Audits.Add(new { record.Id, action = "credential.submitted" }); return Created($"api/credentials/{record.Id}", record);
    }
    [HttpPost("{id:int}/approve")] public IActionResult Approve(int id)
    {
        if (Request.Headers["X-Role"] != "compliance-manager") return StatusCode(StatusCodes.Status403Forbidden, new { error = "Compliance manager role is required" });
        var record = Records.FirstOrDefault(x => x.Id == id); if (record is null) return NotFound(); if (record.Status != "pending") return Conflict(new { error = "Only pending credentials can be approved" });
        var approved = record with { Status = "approved" }; Records[Records.IndexOf(record)] = approved; Audits.Add(new { id, action = "credential.approved" }); return Ok(approved);
    }
    [HttpGet("audit")] public IActionResult Audit() => Ok(Audits);
}
public record CredentialInput(string Contractor, string CredentialType, DateOnly ExpiresOn);
