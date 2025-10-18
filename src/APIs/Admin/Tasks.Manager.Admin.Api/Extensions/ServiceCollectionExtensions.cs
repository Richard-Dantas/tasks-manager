using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Tasks.Manager.Admin.Api.Infra.Configurations;
using Tasks.Manager.Admin.Application.UseCases.Project.Create;
using Tasks.Manager.Admin.Application.UseCases.Project.Delete;
using Tasks.Manager.Admin.Application.UseCases.Project.List;
using Tasks.Manager.Admin.Application.UseCases.Report.Get;
using Tasks.Manager.Admin.Application.UseCases.TaskItem.Create;
using Tasks.Manager.Admin.Application.UseCases.TaskItem.List;
using Tasks.Manager.Admin.Application.UseCases.TaskItem.Remove;
using Tasks.Manager.Admin.Application.UseCases.TaskItem.Update;
using Tasks.Manager.Infrastructure.DependencyInjection;

namespace Tasks.Manager.Admin.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration, string env)
    {
        services
            .AddControllers();

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services
            .AddOpenApi()
            .AddEndpointsApiExplorer();

        services
            .AddUsecases()
            .AddRepositories();

        services
            .AddSwagger()
            .AddCorsConfiguration()
            .AddSqlServerConfiguration(configuration);

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();

        return services;
    }

    private static IServiceCollection AddUsecases(this IServiceCollection services)
    {
        services.AddScoped<ICreateProjectUseCase, CreateProjectUseCase>();
        services.AddScoped<IDeleteProjectUseCase, DeleteProjectUseCase>();
        services.AddScoped<IListProjectUseCase, ListProjectUseCase>();

        services.AddScoped<ICreateTaskItemUseCase, CreateTaskItemUseCase>();
        services.AddScoped<IListTaskItemUseCase, ListTaskItemUseCase>();
        services.AddScoped<IUpdateTaskItemUseCase, UpdateTaskItemUseCase>();
        services.AddScoped<IRemoveTaskItemUseCase, RemoveTaskItemUseCase>();

        services.AddScoped<IGetReportUseCase, GetReportUseCase>();

        return services;
    }

    private static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        return services;
    }
}
