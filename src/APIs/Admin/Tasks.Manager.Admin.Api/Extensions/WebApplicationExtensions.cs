using Tasks.Manager.Admin.Api.Infra.Configurations;
using Tasks.Manager.Infrastructure.DependencyInjection;

namespace Tasks.Manager.Admin.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApp(this WebApplication app)
    {
        ConfigurePathBase(app);

        app.EnsureDatabaseCreated();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseCustomSwagger()
            .ConfigureRedirectToSwagger()
            .UseRouting()
            .UseCors("AllowAll")
            .UseAuthentication()
            .UseHttpsRedirection()
            .UseAuthorization();

        app.MapControllers();

        return app;
    }

    private static void ConfigurePathBase(WebApplication app)
    {
        var pathBase = app.Configuration["HOST_BASE_PATH"];

        if (!string.IsNullOrWhiteSpace(pathBase))
            app.UsePathBase($"/{pathBase}");
    }
}
