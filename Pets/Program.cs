using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Pets.Extensions;
using Pets.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// 1. Determine if running in Docker.
var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

// 2. If local development (not Docker) and in Development environment, load .env.local.
if (builder.Environment.IsDevelopment() && !isDocker)
{
    // Loads environment variables from .env.local into the process.
    Env.Load(".env.local");
}

// 3. Always load environment variables.
builder.Configuration.AddEnvironmentVariables();

// 4. Register application services via our extension method.
builder.Services.ConfigurePetsServices(builder.Configuration);

// Register CORS policy – temporarily allow any origin for testing.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", policyBuilder =>
    {
        policyBuilder
            .AllowAnyOrigin()  // For debugging; later change to .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// 5. Migrate the database on startup.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<Pets.Data.PetDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// For development, disable HTTPS redirection so requests remain HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Use the CORS middleware.
app.UseCors("AllowLocalhost3000");

app.UseAuthentication();
app.UseAuthorization();

// Use custom exception handling middleware.
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
