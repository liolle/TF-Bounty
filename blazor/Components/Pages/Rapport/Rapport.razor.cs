using System.Security.Claims;
using blazor.models;
using blazor.services;
using BlazorMonaco.Editor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace blazor.Components.Pages.Rapport;

public partial class Rapport : ComponentBase
{

    [Parameter]
    [SupplyParameterFromQuery]
    public int? Id { get; set; }

    [Inject]
    IProgramService? programService { get; set; }


    [Inject]
    private NavigationManager? Navigation { get; set; }

    [Inject]
    private AuthenticationStateProvider? AuthProvider { get; set; }

    ProgramModel? SelectedProgram = null;

    StandaloneCodeEditor? Editor {get;set;}

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
            await LoadProgram();
        }
    }

    private async Task LoadProgram(int timeout = 250)
    {
        if (programService is null || Id is null) { return; }
        SelectedProgram = await programService.GetById(Id.Value, timeout);
        StateHasChanged();
    }

    private StandaloneEditorConstructionOptions EditorConstructionOptions(StandaloneCodeEditor editor)
    {
        Editor = editor;
        return new StandaloneEditorConstructionOptions
        {
            Language = "markdown",
            Value = "",
            AutomaticLayout = true,
            AutoIndent = "advanced",
            Theme = "vs-dark",
            AccessibilityPageSize = 1000,
            WordBasedSuggestions = false,
            WordWrap = "on"
        };
    }

    private async Task HandleSubmit(){
        if (Editor is null){return;}
        Console.WriteLine(await Editor.GetValue());
    }
}