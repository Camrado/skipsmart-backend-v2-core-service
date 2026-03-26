using dotenv.net;
using Microsoft.EntityFrameworkCore;
using SkipSmart.Api.Extensions;
using SkipSmart.Api.JsonConverters;
using SkipSmart.Application;
using SkipSmart.Infrastructure;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options => {
    options.AddPolicy("AllowSpecificOrigin", builder => {
        builder
            // .AllowAnyOrigin()
            .WithOrigins("https://skipsmart.org", "https://skipsmart.netlify.app/", "http://localhost:8080")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // dotnet ef migrations add Create_Database --project SkipSmart.Infrastructure --startup-project SkipSmart.Api
    // run this command in the root directory of the solution
    
    // app.SeedData();
}

if (app.Environment.IsProduction())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.ApplyMigrations();

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseRequestContextLogging();

app.UseCustomExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();