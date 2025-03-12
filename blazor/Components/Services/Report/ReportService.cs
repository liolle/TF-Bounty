using blazor.models;
using blazor.utils;
using Microsoft.JSInterop;

namespace blazor.services;

public interface IRapportService
{
    Task Add(ReportModel model);
    Task<List<ReportModel>> GetUserReport(int timeout = 250);
}

public class RapportService(IJSRuntime jSRuntime) : IRapportService
{

    private readonly IJSRuntime _jSRuntime = jSRuntime;
    private readonly Debouncer _debouncer = new Debouncer();

    public async Task Add(ReportModel model)
    {
        await _jSRuntime.InvokeVoidAsync("addRapport", model);
    }

    public async Task<List<ReportModel>> GetUserReport(int timeout = 250)
    {


        TaskCompletionSource<List<ReportModel>> tcs = new();

        _debouncer.Debounce(async () =>
        {
            try
            {
                List<RawReportModel> raw_programs = await _jSRuntime.InvokeAsync<List<RawReportModel>>("getUserReport");
                tcs.SetResult([.. raw_programs.Select(val => val.Extract())]);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }

        }, timeout);

        return await tcs.Task;
    }
}