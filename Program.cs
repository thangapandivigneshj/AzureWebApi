
using Azure.Identity;
using AzureWebApi.Core.DTOs;
using AzureWebApi.Core.Interfaces;
using AzureWebApi.Core.Validators;
using AzureWebApi.Infrastructure.Data;
using AzureWebApi.Infrastructure.Mappings;
using AzureWebApi.Infrastructure.Repositories;
using AzureWebApi.Middleware;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

/// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AzureSqlConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null
            );
        }
    )
);

// Repositories
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<EmployeeMappingProfile>();
});

// FluentValidation
builder.Services.AddScoped<IValidator<EmployeeRequestDto>, EmployeeRequestValidator>();

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();