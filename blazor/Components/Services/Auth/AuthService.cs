
using Microsoft.JSInterop;

namespace blazor.services;

public class AuthService(IJSRuntime js,IConfiguration configuration) : IAuthService
{
  private readonly IJSRuntime _js = js;
  private readonly string API_URL = configuration["API_URL"]?? throw new Exception("Missing configuration: API_URL"); 

  public async Task Logout()
  {
    await _js.InvokeAsync<string>("logout",API_URL);
  }

  public async Task AzureLogin(string code, string redirect_success_uri, string redirect_failure_uri)
  {
    await _js.InvokeVoidAsync("azureOauth", code, redirect_success_uri, redirect_failure_uri,API_URL);
  }

}
