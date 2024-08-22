using dotenv.net;
using Serilog;
using SkipSmart.Api.Extensions;
using SkipSmart.Api.JsonConverters;
using SkipSmart.Application;
using SkipSmart.Infrastructure;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

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
            .AllowAnyOrigin()
            // .WithOrigins("http://localhost:8080")
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
    app.ApplyMigrations();
    
    // app.SeedData();
}

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseCustomExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();