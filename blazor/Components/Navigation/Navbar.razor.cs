namespace blazor.Components.Navigation;

using System.Text.RegularExpressions;
using System.Threading.Tasks;
using blazor.services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

public partial class Navbar : ComponentBase
{
    [Inject]
    private IAuthService? Service { get; set; }

    [Inject]
    private NavigationManager? Navigation { get; set; }
    public bool IsConnected { get; set; }

    [Inject]
    private AuthenticationStateProvider? AuthProvider { get; set; }

    [Inject]
    private IConfiguration? configuration { get; set; }

    public string Page { get; private set; } = "";

    protected override void OnInitialized()
    {
        SetPage();
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        if (AuthProvider is null) { return; }
        var authState = await AuthProvider.GetAuthenticationStateAsync();
        var user = authState.User;
    }

    private void SetPage()
    {
        if (Navigation is null) { return; }

        string[] parts = Navigation.Uri.Split(':');
        if (parts.Length == 0) { return; }
        string pattern = @"\/([^\n\/]*)";
        string input = parts[parts.Length - 1];
        Match match = Regex.Match(input, pattern);
        Page = match.Groups[1].Value.ToLower();
    }

    public void AzureLogin()
    {
        string? redirect_uri = configuration?["REDIRECT_URI"];
        string? scope = configuration?["SCOPE"];
        string? tenant_id = configuration?["MICROSOFT_TENANT_ID"];
        string? client_id = configuration?["MICROSOFT_CLIENT_ID"];

        if (tenant_id is null || client_id is null || scope is null || redirect_uri is null)
        {
            Console.WriteLine("Missing configurations");
            return;
        }

        string URL = $"https://login.microsoftonline.com/{tenant_id}/oauth2/v2.0/authorize?client_id={client_id}&response_type=code&redirect_uri={redirect_uri}&response_mode=query&scope={scope}";
        Navigation?.NavigateTo(URL, false, false);
    }


    public void NavigateToHomePage()
    {
        Navigation?.NavigateTo("/");
    }

    public void NavigateToReport()
    {
        Navigation?.NavigateTo("/myReport");
    }

    public void NavigateToIssues()
    {
        Navigation?.NavigateTo("/issues");
    }


    public async Task Logout()
    {
        if (configuration is null) { return; }
        string? post_logout_redirect_uri = configuration["POST_REDIRECT_URI"];
        if (Service is null || post_logout_redirect_uri is null) { return; }

        await Service.Logout();
        Navigation?.NavigateTo($"https://login.microsoftonline.com/common/oauth2/v2.0/logout?post_logout_redirect_uri={post_logout_redirect_uri}");
    }
}