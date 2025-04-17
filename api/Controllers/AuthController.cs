using api.CQS;
using api.exceptions;
using api.services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace api.controller;

public class AuthController(IUserService userService, IConfiguration configuration, ILogger<AuthController> loger) : ControllerBase
{

  [HttpPost]
  [Route("/oauth/microsoft")]
  [EnableCors("auth-input")]
  public async Task<IActionResult> OauthMicrosoft([FromBody] OauthMicrosoftQuery query)
  {
    try
    {
      string? token_name = configuration["AUTH_TOKEN_NAME"] ?? throw new MissingConfigurationException("AUTH_TOKEN_NAME");
      QueryResult<string> result = await userService.Execute(query);
      if (!result.IsSuccess && query.Redirect_Failure_Uri is not null)
      {
        loger.LogInformation(result.ErrorMessage);
        return BadRequest(result.ErrorMessage);
      }

      if (!result.IsSuccess)
      {
        loger.LogInformation(result.ErrorMessage);
        return BadRequest(result.ErrorMessage);
      }

      CookieOptions cookieOptions = new()
      {
        HttpOnly = true,
                 Secure = true,
                 SameSite = SameSiteMode.Strict,
                 Expires = DateTime.UtcNow.AddMinutes(60)
      };

      loger.LogInformation(result.Result);
      Response.Cookies.Append(token_name, result.Result, cookieOptions);

      return Ok();

    }
    catch (Exception e)
    {
      loger.LogInformation(e.Message);
      return BadRequest("Server error");
    }
  }

  [HttpGet]
  [Route("/logout")]
  [EnableCors("auth-input")]
  public IActionResult Logout()
  {
    try
    {
      string? token_name = configuration["AUTH_TOKEN_NAME"] ?? throw new MissingConfigurationException("AUTH_TOKEN_NAME");
      Response.Cookies.Delete(token_name);
      return Ok();
    }
    catch (Exception e)
    {
      loger.LogInformation(e.Message);
      return BadRequest("Server error");
    }

  }
}
