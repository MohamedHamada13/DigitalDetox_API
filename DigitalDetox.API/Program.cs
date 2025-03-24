using DigitalDetox.Application.Servicies;
using DigitalDetox.Application.Validators;
using DigitalDetox.Core.Context;
using DigitalDetox.Core.DTOs;
using DigitalDetox.Core.Entities;
using DigitalDetox.Core.Interfaces;
using DigitalDetox.Infrastructure.Persistance.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;

var builder = WebApplication.CreateBuilder(args);

/// Add services to the container.
builder.Services.AddControllers()
    .AddFluentValidation(config =>
    {
        // To Prevent Data annotation and provide validators Instead
        config.RegisterValidatorsFromAssemblyContaining<ChallengePostDtoValidator>();
        config.DisableDataAnnotationsValidation = true; 
    });
builder.Services.AddValidatorsFromAssemblyContaining<ChallengePostDtoValidator>();
builder.Services.AddFluentValidationAutoValidation(); // Provide Automatic validation to registered 


builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DegitalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning))); // Provide mapping dynamic values like DateTime.Now

builder.Services.AddScoped<IChallengeRepos, ChallengeRepos>();
builder.Services.AddScoped<IChallengeService, ChallengeService>();
builder.Services.AddScoped<IValidator<ChallengePostDto>, ChallengePostDtoValidator>(); // Register DTO Validator


var app = builder.Build();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
