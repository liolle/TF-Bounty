using blazor.Components;
using blazor.services;
using csrf;
using DotNetEnv;
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

builder.Services.AddSingleton<CsrfService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthProvider>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IProgramService,ProgramService>();
builder.Services.AddScoped<IReportService,ReportService>();
builder.Services.AddScoped<CsrfContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.Use(
    async (context, next) =>
    {

    CsrfService csrfService = context.RequestServices.GetRequiredService<CsrfService>();
    IConfiguration configuration = context.RequestServices.GetRequiredService<IConfiguration>();
    string CSRF_COOKIE_NAME =configuration["CSRF_COOKIE_NAME"]??throw new Exception("Missing configuration CSRF_COOKIE_NAME"); 
    string CSRF_HEADER_NAME =configuration["CSRF_HEADER_NAME"]??throw new Exception("Missing configuration CSRF_HEADER_NAME"); 
    var (cookieToken, requestToken) = csrfService.GenerateTokens();
    var endpoint = context.GetEndpoint();

    if (endpoint?.Metadata.GetMetadata<RequireCsrfTokenAttribute>() != null )
    {

    context.Response.Cookies.Append(
        CSRF_COOKIE_NAME,
        cookieToken,
        new CookieOptions
        {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict
        });

    }
    string cookie = context.Request.Cookies[CSRF_COOKIE_NAME] ?? "";
    context.Items[CSRF_HEADER_NAME] = csrfService.ComputeHmac(cookie);

    await next();
    });

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
  .AddInteractiveServerRenderMode();

  app.MapMetrics(); 
  app.UseStatusCodePagesWithRedirects("/404");
  app.Run();
