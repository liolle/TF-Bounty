using blazor.models;
using Microsoft.JSInterop;

namespace blazor.services;

public interface IProgramService
{
    Task<List<ProgramModel>> GetAll();
}

public class ProgramService(IJSRuntime jSRuntime) : IProgramService
{


    public async Task<List<ProgramModel>> GetAll()
    {
        await Task.CompletedTask;
        List<RawProgramModel> raw_programs = await jSRuntime.InvokeAsync<List<RawProgramModel>>("getAllProgram");
        return [.. raw_programs.Select(val=>val.Extract())];
    }
}