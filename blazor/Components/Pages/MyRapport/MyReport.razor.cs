using System.Security.Claims;
using blazor.models;
using blazor.services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace blazor.Components.Pages.MyRapport;

public partial class MyReport : ComponentBase
{

    [Inject]
    IProgramService? programService { get; set; }

    [Inject]
    IRapportService? rapportService { get; set; }

    [Inject]
    private NavigationManager? Navigation { get; set; }

    [Inject]
    private AuthenticationStateProvider? AuthProvider { get; set; }

    ReportModel? SelectedReport = null;
    ProgramModel? SelectedProgram = null;

    List<ReportModel> reports = [];

    protected override async Task OnInitializedAsync()
    {
        if (AuthProvider is null) { return; }
        var authState = await AuthProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        bool isBountyCreator = user.Claims.Where(claim => claim.Type == ClaimTypes.Role).Any(claim => claim.Value == "Bounty.Hunter");
        if (!isBountyCreator)
        {
            Navigation?.NavigateTo("/");
            return;
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadReports();
        }
    }

    private async Task LoadReports(int timeout = 250)
    {
        if (rapportService is null) { return; }
        reports = await rapportService.GetUserReport(timeout);
        StateHasChanged();
    }

    private async Task HandleReportClick(ReportModel model)
    {
        if (programService is null) { return; }
        if (SelectedReport is not null && model.Id == SelectedReport.Id) { return; }
        SelectedReport = model;
        SelectedProgram = await programService.GetById(model.ProgramId);
        StateHasChanged();
    }
}