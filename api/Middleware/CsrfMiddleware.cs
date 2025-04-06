using csrf;

namespace api.middlewares;

public class CsrfMiddleware 
{

  private readonly RequestDelegate _next;
  private readonly CsrfService _csrfService;

  public CsrfMiddleware(RequestDelegate next, CsrfService csrfService)
  {
    _next = next;
    _csrfService = csrfService;
  }

  public async Task Invoke(HttpContext context)
  {
    if (ShouldValidate(context))
    {
      string cookieToken = context.Request.Cookies[_csrfService.CookieName] ?? "";
      string headerToken = context.Request.Headers[_csrfService.TokenName].FirstOrDefault() ?? "" ;

      if (!_csrfService.ValidateTokens(cookieToken, headerToken))
      {
        context.Response.StatusCode = 403;
        await context.Response.WriteAsync("Invalid CSRF token");
        return;
      }
    }
    await _next(context);
  }

  private bool ShouldValidate(HttpContext context)
  {
    var endpoint = context.GetEndpoint();
    return endpoint?.Metadata.GetMetadata<ValidateCsrfAttribute>() != null;
  }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ValidateCsrfAttribute : Attribute { }
