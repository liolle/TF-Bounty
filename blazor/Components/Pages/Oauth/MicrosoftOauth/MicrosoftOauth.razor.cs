using blazor.services;
using Microsoft.AspNetCore.Components;

namespace blazor.Components.Pages.Oauth.MicrosoftOauth;

[RequireCsrfToken]
public partial class MicrosoftOauth : ComponentBase
{
    [Parameter]
    [SupplyParameterFromQuery]
    public string? Code { get; set; }

    [Inject]
    IAuthService? AuthService {get;set;}

    [Inject]
    NavigationManager? Navigation {get;set;}

    [Inject]
    IConfiguration? Configuration {get;set;}

    [Inject]
    CsrfContext? CContext {get;set;}

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (AuthService is null || Code is null ){
            return;
        }

        string? redirect_url = Configuration?["REDIRECT_URI"];
        string? csrf_token_name = Configuration?["CSRF_HEADER_NAME"];
        if (redirect_url is null || csrf_token_name is null ){return;}

        await AuthService.AzureLogin(Code,redirect_url,redirect_url);

        Navigation?.NavigateTo("/",true,true);
    }
}
