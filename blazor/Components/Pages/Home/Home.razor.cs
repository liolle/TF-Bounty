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

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadPrograms();
            rendered_once = true;
        }

    }

    protected override async Task OnParametersSetAsync()
    {
        if (rendered_once)
        {
            await LoadPrograms(1000);
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
}