using blazor.models;
using blazor.utils;
using Microsoft.JSInterop;

namespace blazor.services;

public interface IRapportService
{
    Task Add(RapportModel model);
}

public class RapportService(IJSRuntime jSRuntime) : IRapportService
{

    private readonly IJSRuntime _jSRuntime = jSRuntime;

    public async Task Add(RapportModel model)
    {
        await _jSRuntime.InvokeVoidAsync("addRapport", model);
    }
}