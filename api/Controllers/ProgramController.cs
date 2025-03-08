using System.Security.Claims;
using api.CQS;
using api.database.entities;
using api.models;
using api.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace api.controller;

public class ProgramController(IProgramService programService) : ControllerBase
{

    [HttpPost]
    [Route("/program/create")]
    [Authorize(Roles = "Bounty.Creator")]
    [EnableCors("auth-input")]
    public async Task<IActionResult> Create([FromBody] AddProgramModel model)
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

        CommandResult result = programService.Execute(
            new CreateProgramCommand(
                oid,
                model.Logo ?? "",
                model.Reward,
                model.Title,
                model.Description ?? ""
            )
        );

        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }

        await Task.CompletedTask;
        return Ok();
    }

    [HttpGet]
    [Route("/program/get/me")]
    [Authorize(Roles = "Bounty.Creator")]
    [EnableCors("auth-input")]
    public async Task<IActionResult> GetUserPrograms()
    {

        string? oid = User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Oid)?.Value;
        if (oid is null)
        {
            return BadRequest("Malformed token");
        }

        QueryResult<List<ProgramEntity>> result = programService.Execute(new GetUserProgramsQuery(oid));

        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }

        await Task.CompletedTask;
        return Ok(result.Result);
    }



    [HttpGet]
    [Route("/program/get/all")]
    [EnableCors("auth-input")]
    public async Task<IActionResult> GetAllPrograms()
    {
        QueryResult<List<ProgramEntity>> result = programService.Execute(new GetProgramsQuery());

        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }

        await Task.CompletedTask;
        return Ok(result.Result);
    }
}