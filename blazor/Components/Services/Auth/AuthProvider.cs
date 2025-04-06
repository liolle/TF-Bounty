using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace blazor.services;

public class AuthProvider(IHttpContextAccessor httpContextAccessor) : AuthenticationStateProvider
{
  public override async Task<AuthenticationState> GetAuthenticationStateAsync()
  {

    HttpContext? httpContext = httpContextAccessor.HttpContext;

    if (httpContext is null)
    {
      return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    string? token = httpContext.Request.Cookies["bounty_auth_token"];

    if (token is null)
    {
      return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    JwtSecurityTokenHandler handler = new();
    JwtSecurityToken jsonToken = handler.ReadJwtToken(token);

    string? oid = jsonToken.Claims.FirstOrDefault(c => c.Type == "oid")?.Value;

    if (oid is null){
      return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    IEnumerable<Claim>roles = jsonToken.Claims.Where(val=>val.Type =="roles");

    List<Claim> claims = [
      new("oid",oid)
    ];

    foreach (Claim item in roles)
    {
      claims.Add(new Claim(ClaimTypes.Role,item.Value));
    }

    ClaimsIdentity identity = new(claims, "cookieAuth");
    await Task.CompletedTask;
    return new AuthenticationState(new ClaimsPrincipal(identity));
  }

}
