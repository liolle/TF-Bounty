using blazor.models;
using blazor.utils;
using Microsoft.JSInterop;

namespace blazor.services;

public interface IReportService
{
    Task Add(ReportModel model);
    Task<List<ReportModel>> GetUserReport(int timeout = 250);
    Task<List<ReportModel>> GetPendingReport(int timeout = 250);
    Task<bool> ValidateReport(string state, int id);
}

public class ReportService(IJSRuntime jSRuntime, IHttpContextAccessor contextAccessor, IConfiguration config) : IReportService
{

    private readonly IJSRuntime _jSRuntime = jSRuntime;
    private readonly Debouncer _debouncer = new Debouncer();
    private readonly HttpContext _contextAccessor = contextAccessor?.HttpContext ?? throw new Exception("Missing HttpContext");
    private readonly string _tokenName = config["CSRF_HEADER_NAME"]??throw new Exception("Missing configuration CSRF_HEADER_NAME");
  
    private string CSRF_TOKEN {get{
      string? t = _contextAccessor.Items[_tokenName] as string;
      return t??"";
    }}

    public async Task Add(ReportModel model)
    {
      await _jSRuntime.InvokeVoidAsync("addRapport", model,CSRF_TOKEN);
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

    public async Task<List<ReportModel>> GetPendingReport(int timeout = 250)
    {

      TaskCompletionSource<List<ReportModel>> tcs = new();

      _debouncer.Debounce(async () =>
          {
          try
          {
          List<RawReportModel> raw_programs = await _jSRuntime.InvokeAsync<List<RawReportModel>>("getPendingReport");
          tcs.SetResult([.. raw_programs.Select(val => val.Extract())]);
          }
          catch (Exception e)
          {
          tcs.SetException(e);
          }

          }, timeout);

      return await tcs.Task;
    }

    public async Task<bool> ValidateReport(string state, int id)
    {
      TaskCompletionSource<bool> tcs = new();

      _debouncer.Debounce(async () =>
          {
          try
          {
          bool res = await _jSRuntime.InvokeAsync<bool>("validateReport", state, id,CSRF_TOKEN);
          tcs.SetResult(res);
          }
          catch (Exception e)
          {
          tcs.SetException(e);
          }
          });

      return await tcs.Task;
    }
}
