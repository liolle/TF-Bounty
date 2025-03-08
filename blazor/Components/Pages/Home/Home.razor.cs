using blazor.models;
using blazor.services;
using Microsoft.AspNetCore.Components;

namespace blazor.Components.Pages.Home;


public partial class Home
{
    [Inject]
    IProgramService? programService {get;set;}

    List<ProgramModel> programs = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && programService is not null){
            programs = await programService.GetAll();
            StateHasChanged();
        }
        
    }
}