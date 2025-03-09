using blazor.models;
using blazor.utils;
using Microsoft.JSInterop;

namespace blazor.services;

public interface IProgramService
{
    Task<List<ProgramModel>> GetAll(string search="",int timeout = 250);
}

public class ProgramService : IProgramService, IDisposable
{

    private readonly IJSRuntime _jSRuntime;
    private readonly Debouncer _debouncer;

    public ProgramService(IJSRuntime jSRuntime)
    {
        _jSRuntime = jSRuntime;
        _debouncer = new Debouncer();
    }


    public async Task<List<ProgramModel>> GetAll(string search="",int timeout = 250)
    {
        TaskCompletionSource<List<ProgramModel>> tcs = new();

        _debouncer.Debounce(async () =>
        {
            try
            {
                List<RawProgramModel> raw_programs = await _jSRuntime.InvokeAsync<List<RawProgramModel>>("getAllProgram",search);
                tcs.SetResult([.. raw_programs.Select(val => val.Extract())]);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }

        }, timeout);

        return await tcs.Task;
    }


    public void Dispose()
    {
        _debouncer.Dispose();
    }
}