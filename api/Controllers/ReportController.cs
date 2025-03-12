using api.CQS;
using api.database.entities;
using api.models;
using api.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace api.controller;

public class ReportController(IReportService reportService) : ControllerBase
{

    [HttpPost]
    [Route("/report/create")]
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

        CommandResult result = reportService.Execute(
            new CreateRapportCommand(
                model.Title,
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


    [HttpGet]
    [Route("/report/get/me")]
    [Authorize(Roles = "Bounty.Hunter")]
    [EnableCors("auth-input")]
    public async Task<IActionResult> GetUserReport()
    {

        string? oid = User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Oid)?.Value;
        if (oid is null)
        {
            return BadRequest("Malformed token");
        }

        QueryResult<List<ReportEntity>> result = reportService.Execute(new GetUserReportQuery(oid));

        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }

        await Task.CompletedTask;
        return Ok(result.Result);
    }

    [HttpGet]
    [Route("/report/get/pending")]
    [Authorize(Roles = "Admin")]
    [EnableCors("auth-input")]
    public async Task<IActionResult> GetPendingReport()
    {


        QueryResult<List<ReportEntity>> result = reportService.Execute(new GetPendingReportQuery());

        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }

        await Task.CompletedTask;
        return Ok(result.Result);
    }
}