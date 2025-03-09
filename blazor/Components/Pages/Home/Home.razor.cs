using blazor.models;
using blazor.services;
using Microsoft.AspNetCore.Components;

namespace blazor.Components.Pages.Home;


public partial class Home
{
    [Inject]
    IProgramService? programService { get; set; }

    List<ProgramModel> programs = [];
    private string? _searchQuery;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadPrograms();
        }

    }

    private async Task LoadPrograms(int timeout = 250)
    {
        if (programService is null) { return; }
        programs = await programService.GetAll(timeout);
        StateHasChanged();
    }

    private async Task OnSearchInput(ChangeEventArgs e)
    {
        _searchQuery = e.Value?.ToString();
        await LoadPrograms();
    }
}