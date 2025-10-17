using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tasks.Manager.Infrastructure.Data;

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
}
