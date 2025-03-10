using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using blazor.models;
using blazor.services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace blazor.Components.Pages.Program;

public partial class Program : ComponentBase
{
    [Inject]
    IProgramService? programService { get; set; }

    [Inject]
    private NavigationManager? Navigation { get; set; }

    [Inject]
    private AuthenticationStateProvider? AuthProvider { get; set; }

    public ProgramModel Model { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        if (AuthProvider is null) { return; }
        var authState = await AuthProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        bool isBountyCreator = user.Claims.Where(claim => claim.Type == ClaimTypes.Role).Any(claim => claim.Value == "Bounty.Creator");
        if (!isBountyCreator)
        {
            Navigation?.NavigateTo("/");
            return;
        }

    }

    private async Task HandleValidSubmit()
    {
        if (programService is null){return;}
        await programService.Add(Model);
        Navigation?.NavigateTo("/");
    }

}

