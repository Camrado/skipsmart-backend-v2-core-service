using System.Text;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Clock;
using SkipSmart.Application.Abstractions.Data;
using SkipSmart.Application.Abstractions.Email;
using SkipSmart.Application.Abstractions.Timetable;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Attendances;
using SkipSmart.Domain.CourseHours;
using SkipSmart.Domain.Courses;
using SkipSmart.Domain.Groups;
using SkipSmart.Domain.MarkedDates;
using SkipSmart.Domain.Users;
using SkipSmart.Infrastructure.Authentication;
using SkipSmart.Infrastructure.Clock;
using SkipSmart.Infrastructure.Data;
using SkipSmart.Infrastructure.Email;
using SkipSmart.Infrastructure.Repositories;
using SkipSmart.Infrastructure.Timetable;

namespace SkipSmart.Infrastructure;

public static class DependencyInjection {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();

        services.AddTransient<IEmailService, EmailService>();
        
        AddPersistence(services);
        
        AddAuthentication(services, configuration);
        
        AddAuthorization(services);
        
        AddTimetable(services, configuration);
        
        return services;
    }
    
    private static void AddPersistence(IServiceCollection services) {
        // TODO: Add database connection string to .env file
        var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ??
                               throw new ApplicationException("Database connection string is missing.");

        services.AddDbContext<ApplicationDbContext>(options => {
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IUserRepository, UserRepository>();
        
        services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        
        services.AddScoped<ICourseHourRepository, CourseHourRepository>();

        services.AddScoped<ICourseRepository, CourseRepository>();
        
        services.AddScoped<IGroupRepository, GroupRepository>();

        services.AddScoped<IMarkedDateRepository, MarkedDateRepository>();
        
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));

        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
    }

    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration) {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters() {
                    ValidIssuer = configuration["Authentication:Issuer"],
                    ValidAudience = configuration["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET") 
                                  ?? throw new ApplicationException("Jwt secret is missing."))),
                    
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });
        
        services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));

        services.AddTransient<IJwtService, JwtService>();
        
        services.AddHttpContextAccessor();
        
        services.AddScoped<IUserContext, UserContext>();

        services.AddTransient<IEmailVerificationService, EmailVerificationService>();
    }

    private static void AddAuthorization(IServiceCollection services) {
        services.AddAuthorization(options => {
            options.AddPolicy("EmailVerified", policy =>
                policy.RequireClaim("email_verified", "true"));
        });
    }

    private static void AddTimetable(IServiceCollection services, IConfiguration configuration) {
        services.Configure<TimetableOptions>(configuration.GetSection("Timetable"));
        
        services.AddHttpClient<ITimetableService, TimetableService>((serviceProvider, httpClient) => {
            // var timetableOptions = serviceProvider.GetRequiredService<IOptions<TimetableOptions>>().Value;

            // httpClient.BaseAddress = new Uri(timetableOptions.BaseUrl);
        });
    }
}