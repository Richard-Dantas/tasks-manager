using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tasks.Manager.Domain.Repositories;
using Tasks.Manager.Infrastructure.Data;
using Tasks.Manager.Infrastructure.Persistence.SqlServer.Repositories;

namespace Tasks.Manager.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSqlServerConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        var dbName = Environment.GetEnvironmentVariable("DB_NAME");
        var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

        var connectionString =
            configuration.GetConnectionString("DefaultConnection") ??
            $"Server={dbHost ?? "localhost"};Database={dbName ?? "TaskManagerApp"};User Id=sa;Password={dbPassword ?? "senha123!"};Encrypt=False;TrustServerCertificate=True;";

        services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
