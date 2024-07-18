using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkipSmart.Application.Abstractions.Data;
using SkipSmart.Domain.Attendances;
using SkipSmart.Domain.CourseHours;
using SkipSmart.Domain.Courses;
using SkipSmart.Domain.Groups;
using SkipSmart.Domain.MarkedDates;
using SkipSmart.Domain.Users;
using SkipSmart.Infrastructure.Data;
using SkipSmart.Infrastructure.Repositories;

namespace SkipSmart.Infrastructure;

public static class DependencyInjection {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
        AddPersistence(services, configuration);
        
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
}