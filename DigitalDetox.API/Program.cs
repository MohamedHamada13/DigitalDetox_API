using DigitalDetox.Application.Servicies;
using DigitalDetox.Application.Validators;
using DigitalDetox.Core.Context;
using DigitalDetox.Core.Interfaces;
using DigitalDetox.Infrastructure.Persistance.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DigitalDetox.Core.Entities.Models;
using DigitalDetox.Core.DTOs.ChallengeDto;
using DigitalDetox.Core.Entities.AuthModels;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

/// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<ChallengePostDtoValidator>();
builder.Services.AddFluentValidationAutoValidation(); // Provide Automatic validation to registered 


builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DegitalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning))); // Provide mapping dynamic values like DateTime.Now

builder.Services.AddScoped<IDailyUsageLogService, DailyUsageLogService>();
builder.Services.AddScoped<IChallengeRepos, ChallengeRepos>();
builder.Services.AddScoped<IChallengeService, ChallengeService>();
builder.Services.AddScoped<IValidator<ChallengePostDto>, ChallengePostDtoValidator>(); // Register DTO Validator
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserStoreTemporaryRepos, UserStoreTemporaryRepos>();
builder.Services.AddScoped<IAppRepos, AppRepos>();
builder.Services.AddScoped<IDailyUsageLogRepos, DailyUsageLogRepos>();
builder.Services.AddScoped<IOtpCodeRepos, OtpCodeRepos>();
builder.Services.AddHttpContextAccessor(); // IHttpContextAccessor used to get the user details using token. 

// Rate limiter
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("OtpPolicy", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 3, // allow 3 requests
                Window = TimeSpan.FromMinutes(1), // per minute
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 2 // optional: how many to queue before rejecting
            }));
});


// Add CORS Services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Enable Email Confirmation in Identity Config
builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
});


// Configure Identity
builder.Services.AddIdentity<AppUser, IdentityRole>() 
    .AddEntityFrameworkStores<DegitalDbContext>()
    .AddDefaultTokenProviders();

// Auth Configurations
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT")); // Map appsetting JWT values into JWT class.
builder.Services.AddAuthentication(options => // Configure the default Schema
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}) // Add Authentication Options
    .AddJwtBearer(o =>
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        o.RequireHttpsMetadata = false;
        o.SaveToken = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            RoleClaimType = ClaimTypes.Role
        };
    });

var app = builder.Build();

// Seeding Roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = new[] { "Admin", "User", "Manager" };

    foreach (var role in roles)
    {
        var roleExists = await roleManager.RoleExistsAsync(role);
        if (!roleExists)
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "CurrencyExchange API v1");
        options.RoutePrefix = string.Empty; // This makes Swagger available at root URL
    });
}

app.UseRateLimiter();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
