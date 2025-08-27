using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using CMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;    

Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger (removed JWT authentication since external auth is used)
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Asset Track Plus - Corporate Management System API",
        Version = "v1",
        Description = "Comprehensive Corporate Management System API for Asset Tracking, Expense Management, and More. Authentication handled externally.",
        Contact = new OpenApiContact
        {
            Name = "Asset Track Plus Team",
            Email = "support@assettrackplus.com"
        }
    });
});

// Configure CORS for external authentication integration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173", "http://localhost:3000")  // Add your React frontend URLs
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();  // Required for cookies/auth
    });
});

// Configure PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Services (removed AuthService since external auth is used)
builder.Services.AddScoped<CMS.API.Services.DataSeedingService>();
// Add other application services here as needed

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Asset Track Plus API v1");
        c.RoutePrefix = string.Empty; // Make Swagger the default page
    });
}

app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowAll");

// No authentication middleware needed since handled externally

app.MapControllers();

// Ensure database is created (removed user seeding since users come from external system)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    try
    {
        // Ensure database is created
        await context.Database.EnsureCreatedAsync();
        
        Console.WriteLine("Database initialized successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database initialization error: {ex.Message}");
    }
}

app.Urls.Add("http://localhost:5000");
app.Run();

