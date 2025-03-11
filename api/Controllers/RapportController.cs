using api.CQS;
using api.models;
using api.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace api.controller;

public class RapportController(IRapportService rapportService) : ControllerBase
{
    
    [HttpPost]
    [Route("/rapport/create")]
    [Authorize(Roles = "Bounty.Hunter")]
    [EnableCors("auth-input")]
    public async Task<IActionResult> Create([FromBody] AddRapportModel model)
    {
        string? oid = User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Oid)?.Value;
        if (oid is null)
        {
            return BadRequest("Malformed token");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid model");
        }

        CommandResult result = rapportService.Execute(
            new CreateRapportCommand(
                model.ProgramId,
                oid,
                model.Content
            )
        );

        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }
        
        await Task.CompletedTask;
        return Ok();
    }
}