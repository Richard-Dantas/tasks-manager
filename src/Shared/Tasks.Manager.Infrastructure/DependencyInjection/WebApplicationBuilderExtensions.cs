using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Tasks.Manager.Infrastructure.Data;

namespace Tasks.Manager.Infrastructure.DependencyInjection;

public static class WebApplicationBuilderExtensions
{
    public static void EnsureDatabaseCreated(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        db.Database.EnsureCreated();
    }

    public static void EnsureSeedData(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        context.Seed();
    }
}
