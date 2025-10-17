using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Tasks.Manager.Admin.Api.Infra.Configurations;

public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

    public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        => _apiVersionDescriptionProvider = apiVersionDescriptionProvider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            if (!options.SwaggerGeneratorOptions.SwaggerDocs.ContainsKey(description.GroupName))
            {
                options.SwaggerDoc(description.GroupName, CreateOpenApiInfo(description));
            }
        }
    }

    private static OpenApiInfo CreateOpenApiInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "Tasks Manager API",
            Version = description.ApiVersion.ToString(),
            Description = "API de gerenciamento de tarefas."
        };

        if (description.IsDeprecated)
            info.Description += " (deprecated)";

        return info;
    }
}
