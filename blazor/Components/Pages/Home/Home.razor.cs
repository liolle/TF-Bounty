using blazor.models;
using blazor.services;
using Microsoft.AspNetCore.Components;

namespace blazor.Components.Pages.Home;


public partial class Home
{
    [Inject]
    IProgramService? programService { get; set; }

    [Inject]
    NavigationManager? _navigation { get; set; }

    List<ProgramModel> programs = [];

    [Parameter]
    [SupplyParameterFromQuery]
    public string? search { get; set; }

    [Inject]
    private NavigationManager? Navigation { get; set; }

    bool rendered_once = false;

    ProgramModel? SelectedProgram = null;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadPrograms();
            rendered_once = true;
        }
    }

    private async Task LoadPrograms(int timeout = 250)
    {
        if (programService is null) { return; }
        programs = await programService.GetAll(search ?? "", timeout);
        StateHasChanged();
    }

    private void CreateProgram()
    {
        Navigation?.NavigateTo("/program/create");
    }

    private void CreateRapport()
    {
        if (SelectedProgram is null){return;}
        Navigation?.NavigateTo($"/rapport/create?id={SelectedProgram.Id}");
    }

    private async Task OnSearchInput(ChangeEventArgs e)
    {
        if (_navigation is null) { return; }
        string? value = e.Value?.ToString();
        if (value is not null && value != "")
        {
            _navigation.NavigateTo($"/?search={value}");
        }
        else
        {
            _navigation.NavigateTo($"/");
        }
        await Task.CompletedTask;
    }

    private void HandleProgramClick(ProgramModel model)
    {
        SelectedProgram = model;
        StateHasChanged();
    }

    private void RemoveSelected()
    {
        SelectedProgram = null;
        StateHasChanged();
    }
}