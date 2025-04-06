
using Microsoft.JSInterop;

namespace blazor.services;

public class AuthService(IJSRuntime jS) : IAuthService
{

    private readonly IJSRuntime JS = jS;

    public async Task Logout()
    {
        await JS.InvokeAsync<string>("logout");
    }

    public async Task AzureLogin(string code, string redirect_success_uri, string redirect_failure_uri)
    {
        await JS.InvokeVoidAsync("azureOauth", code, redirect_success_uri, redirect_failure_uri);
    }
}
