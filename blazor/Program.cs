using blazor.Components;
using blazor.services;
using DotNetEnv;
using edllx.dotnet.csrf;
using Microsoft.AspNetCore.Components.Authorization;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add Env &  Json configuration
Env.Load();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseCSRFBlazorServer();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
  .AddInteractiveServerRenderMode();

  app.MapMetrics(); 
  app.UseStatusCodePagesWithRedirects("/404");
  app.Run();
