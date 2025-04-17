using System.Text;
using api.database;
using api.services;
using DotNetEnv;
using edllx.dotnet.csrf;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add Env &  Json configuration
Env.Load();
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Add services to the container.

//JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options=>{
        string? jwt_key = configuration["JWT_KEY"] ?? throw new Exception("Missing JWT_KEY configuration");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["JWT_ISSUER"],
            ValidAudience = configuration["JWT_AUDIENCE"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt_key))
        };

        // extract token from cookies and place it into the Authorization header.
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                string? jwt_name = configuration["AUTH_TOKEN_NAME"] ?? throw new Exception("Missing AUTH_TOKEN_NAME configuration");
                if (context.Request.Cookies.ContainsKey(jwt_name))
                {
                    context.Token = context.Request.Cookies[jwt_name];
                }
                return Task.CompletedTask;
            }
        };
    });

string front_host = configuration["FRONT_HOST"] ?? throw new Exception("Missing configuration: FRONT_HOST") ;

// Cors
builder.Services.AddCors(options=>{
    options.AddPolicy("auth-input", policy=>{
        policy
        .WithOrigins([front_host])
        //.AllowAnyOrigin()
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<CSRFService>();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<IJwtService,JwtService>();
builder.Services.AddScoped<IDataContext,DataContext>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IProgramService,ProgramService>();
builder.Services.AddScoped<IReportService,ReportService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthorization();
app.UseCSRFApi();

app.MapControllers();
app.Run();
