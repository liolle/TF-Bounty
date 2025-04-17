using blazor.Components;
using blazor.services;
using DotNetEnv;
using edllx.dotnet.csrf;
using Microsoft.AspNetCore.Components.Authorization;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add Env &  Json configuration
Env.Load();
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddSingleton<CSRFService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthProvider>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IProgramService,ProgramService>();
builder.Services.AddScoped<IReportService,ReportService>();

var app = builder.Build();

app.UseStaticFiles();


app.UseCSRFBlazorServer();
app.UseAntiforgery();

app.MapRazorComponents<App>()
  .AddInteractiveServerRenderMode();

  app.MapMetrics(); 
  app.UseStatusCodePagesWithRedirects("/404");
  app.Run();
