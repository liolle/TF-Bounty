using blazor.models;
using blazor.utils;
using Microsoft.JSInterop;

namespace blazor.services;

public interface IProgramService
{
  Task<List<ProgramModel>> GetAll(string search = "", int timeout = 250);
  Task<ProgramModel?> GetById(int id, int timeout = 250);
  Task Add(ProgramModel model);
}

public class ProgramService(IJSRuntime jSRuntime, IHttpContextAccessor contextAccessor, IConfiguration config) : IProgramService, IDisposable
{

  private readonly IJSRuntime _jSRuntime = jSRuntime;
  private readonly Debouncer _debouncer = new Debouncer();
  private readonly HttpContext _contextAccessor = contextAccessor?.HttpContext ?? throw new Exception("Missing HttpContext");
  private readonly string _tokenName = config["CSRF_HEADER_NAME"]??throw new Exception("Missing configuration CSRF_HEADER_NAME");
  private readonly string API_URL = config["API_URL"]?? throw new Exception("Missing configuration: API_URL"); 


  private string CSRF_TOKEN {get{
    string? t = _contextAccessor.Items[_tokenName] as string;
    return t??"";
  }}

  public async Task<List<ProgramModel>> GetAll(string search = "", int timeout = 250)
  {
    TaskCompletionSource<List<ProgramModel>> tcs = new();

    _debouncer.Debounce(async () =>
        {
        try
        {
        List<RawProgramModel> raw_programs = await _jSRuntime.InvokeAsync<List<RawProgramModel>>("getAllProgram", search,API_URL);
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

  public async Task Add(ProgramModel model)
  {
    await _jSRuntime.InvokeVoidAsync("addProgram", model,CSRF_TOKEN,API_URL);
  }

  public async Task<ProgramModel?> GetById(int id, int timeout = 250)
  {
    TaskCompletionSource<ProgramModel?> tcs = new();

    _debouncer.Debounce(async () =>{
        try
        {
          RawProgramModel? raw_programs = await _jSRuntime.InvokeAsync<RawProgramModel?>("getProgramById", id,API_URL);
          if (raw_programs is not null)
          {
          tcs.SetResult(raw_programs.Extract());
          }
          else
          {
          tcs.SetResult(null);
          }
        }
        catch (Exception e)
        {
          tcs.SetException(e);
        }
    }, 
        timeout);
    return await tcs.Task;
  }
}
