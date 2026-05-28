using AzureWebApi.Services;
using AzureWebApi.Services.Interfaces;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// CORS: permissive "AllowAll" policy (AllowAnyOrigin / AllowAnyMethod / AllowAnyHeader)
var corsPolicyName = "AllowAll";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Employee API",
        Version = "v1",
        Description = "Enterprise Employee Management API"
    });
});
// Register services
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddApplicationInsightsTelemetry(new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions
{
    ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee API v1");
        //options.RoutePrefix = string.Empty; // opens Swagger at root: https://localhost:xxxx/
    });
//}

app.UseHttpsRedirection();

// Enable CORS (must be before UseAuthorization / MapControllers)
app.UseCors(corsPolicyName);

app.UseAuthorization();

app.MapControllers();

app.Run();
