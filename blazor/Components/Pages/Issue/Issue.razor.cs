using System.Security.Claims;
using System.Threading.Tasks;
using blazor.models;
using blazor.services;
using BlazorMonaco.Editor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace blazor.Components.Pages.Issue;

public partial class Issue : ComponentBase
{
    [Inject]
    IProgramService? programService { get; set; }

    [Inject]
    IReportService? reportService { get; set; }

    [Inject]
    private NavigationManager? Navigation { get; set; }

    [Inject]
    private AuthenticationStateProvider? AuthProvider { get; set; }

    ReportModel? SelectedReport = null;
    ProgramModel? SelectedProgram = null;

    List<ReportModel> reports = [];

    StandaloneCodeEditor? Editor { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (AuthProvider is null) { return; }
        var authState = await AuthProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        bool isBountyCreator = user.Claims.Where(claim => claim.Type == ClaimTypes.Role).Any(claim => claim.Value == "Admin");
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
        if (reportService is null) { return; }
        reports = await reportService.GetPendingReport(timeout);
        StateHasChanged();
    }

    private async Task UpdateSelection(ReportModel model)
    {
        if (programService is null) { return; }
        if (SelectedReport is not null && model.Id == SelectedReport.Id) { return; }
        SelectedReport = model;
        SelectedProgram = await programService.GetById(model.ProgramId, 500);
    }

    private async Task HandleReportClick(ReportModel model)
    {


        await UpdateSelection(model);
        if (Editor is not null && SelectedReport is not null)
        {
            await Editor.SetValue(SelectedReport.Content);
        }
        StateHasChanged();

    }

    private void RemoveSelected()
    {
        SelectedProgram = null;
        SelectedReport = null;
        StateHasChanged();
    }

    private async Task ValidateReport()
    {
        if (reportService is null || SelectedReport is null)
        {
            return;
        }

        if (await reportService.ValidateReport("validated", SelectedReport.Id))
        {
            RemoveReport(SelectedReport.Id);
        }
    }

    private async Task RejectReport()
    {
        if (reportService is null || SelectedReport is null)
        {
            return;
        }

        if (await reportService.ValidateReport("rejected", SelectedReport.Id))
        {
            RemoveReport(SelectedReport.Id);
        }
    }

    private void RemoveReport(int id)
    {
        reports = reports.Where(val => val.Id != id).ToList();
        RemoveSelected();
        StateHasChanged();
    }

    private StandaloneEditorConstructionOptions EditorConstructionOptions(StandaloneCodeEditor editor)
    {
        Editor = editor;

        return new StandaloneEditorConstructionOptions
        {
            Language = "markdown",
            Value = SelectedReport?.Content ?? "",
            AutomaticLayout = true,
            AutoIndent = "advanced",
            Theme = "vs-dark",
            AccessibilityPageSize = 1000,
            WordBasedSuggestions = false,
            WordWrap = "on",
            ReadOnly = true,
            Model = null
        };
    }
}