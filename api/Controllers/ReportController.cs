using api.CQS;
using api.database.entities;
using api.middlewares;
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
    [ValidateCsrf]
    public IActionResult Create([FromBody] AddRapportModel model)
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

        return Ok();
    }


    [HttpGet]
    [Route("/report/get/me")]
    [Authorize(Roles = "Bounty.Hunter")]
    [EnableCors("auth-input")]
    public IActionResult GetUserReport()
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

        return Ok(result.Result);
    }

    [HttpGet]
    [Route("/report/get/pending")]
    [Authorize(Roles = "Admin")]
    [EnableCors("auth-input")]
    public IActionResult GetPendingReport()
    {

        QueryResult<List<ReportEntity>> result = reportService.Execute(new GetPendingReportQuery());

        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok(result.Result);
    }

    [HttpPatch]
    [Route("/report/validate")]
    [Authorize(Roles = "Admin")]
    [EnableCors("auth-input")]
    [ValidateCsrf]
    public IActionResult ValidateReport([FromQuery] string state, [FromQuery] int? id)
    {

        if (id is null)
        {
            return BadRequest("Missing report id");
        }

        if (state is null || (state != "validated" && state != "rejected"))
        {
            return BadRequest("Invalid query state, it should be 'validated' or 'rejected'");
        }

        CommandResult result = reportService.Execute(new ValidateReportCommand(id.Value, state));

        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok();
    }

}
