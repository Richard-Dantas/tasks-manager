using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Tasks.Manager.Admin.Api.Infra.Configurations;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["controller"]}_{e.RelativePath}_{e.HttpMethod}");
            options.DescribeAllParametersInCamelCase();
            options.IncludeXmlComments(XmlCommentsFilePath);

            options.OperationFilter<FormDataOperationFile>();
        });

        return services;
    }

    public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
    {
        var versionDescriptionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in versionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    $"Web APi - {description.GroupName.ToUpper()}");
            }
        });
        return app;
    }

    public static IApplicationBuilder ConfigureRedirectToSwagger(this IApplicationBuilder app)
    {
        var wordsToConsider = new[] { "docs", "documentation", "doc" };

        var rewriterOptions = new RewriteOptions().AddRedirect(
            @$"^$|\b({string.Join('|', wordsToConsider)})",
            "swagger"
        );

        app.UseRewriter(rewriterOptions);

        return app;
    }

    private static string XmlCommentsFilePath
    {
        get
        {
            var programAssembly = typeof(Program).Assembly;
            var basePath = AppContext.BaseDirectory;
            var fileName = $"{programAssembly.GetName().Name}.xml";
            return Path.Combine(basePath!, fileName);
        }
    }

    private sealed class FormDataOperationFile : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var formFileParameters = context.MethodInfo.GetParameters()
                .Where(p => p.ParameterType == typeof(IFormFile))
                .Select(p => p.Name)
                .ToArray();

            if (formFileParameters.Length != 0)
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content =
                    {
                        ["multipart/form-data"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = formFileParameters.ToDictionary(
                                    name => name,
                                    name => new OpenApiSchema
                                    {
                                        Type = "string",
                                        Format = "binary"
                                    }
                                )
                            }
                        }
                    }
                };
            }
        }
    }
}
