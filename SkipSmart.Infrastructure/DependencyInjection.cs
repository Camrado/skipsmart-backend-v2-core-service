using System.Text;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Clock;
using SkipSmart.Application.Abstractions.Data;
using SkipSmart.Domain.Attendances;
using SkipSmart.Domain.CourseHours;
using SkipSmart.Domain.Courses;
using SkipSmart.Domain.Groups;
using SkipSmart.Domain.MarkedDates;
using SkipSmart.Domain.Users;
using SkipSmart.Infrastructure.Authentication;
using SkipSmart.Infrastructure.Clock;
using SkipSmart.Infrastructure.Data;
using SkipSmart.Infrastructure.Repositories;

namespace SkipSmart.Infrastructure;

public static class DependencyInjection {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        
        AddPersistence(services, configuration);
        
        AddAuthentication(services, configuration);
        
        return services;
    }
    
    private static void AddPersistence(IServiceCollection services, IConfiguration configuration) {
        var connectionString = configuration.GetConnectionString("Database") ??
                               throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options => {
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IUserRepository, UserRepository>();
        
        services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        
        services.AddScoped<ICourseHourRepository, CourseHourRepository>();

        services.AddScoped<ICourseRepository, CourseRepository>();
        
        services.AddScoped<IGroupRepository, GroupRepository>();

        services.AddScoped<IMarkedDateRepository, MarkedDateRepository>();

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
                        .GetBytes(Environment.GetEnvironmentVariable("JwtSecret") 
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
    }
}