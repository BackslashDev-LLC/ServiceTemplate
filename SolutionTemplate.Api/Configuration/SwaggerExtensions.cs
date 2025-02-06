using Asp.Versioning;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace SolutionTemplate.Api.Configuration
{
    internal static class SwaggerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen(c =>
                {
                    c.DescribeAllParametersInCamelCase();

                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

                    // Names used here are used in URL for SwaggerUI
                    c.SwaggerDoc("v1.0", new OpenApiInfo { Title = "SolutionTemplate API", Version = "v1.0" });

                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "bearer"
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });

                    c.OperationFilter<HttpHeadOperationFilter>();

                    // Determine which set of documentation an API should belong to
                    c.DocInclusionPredicate((docName, apiDesc) =>
                    {
                        ApiVersionModel? actionApiVersionModel = apiDesc.ActionDescriptor?.GetApiVersion();

                        // if no version is specified or API is marked version neutral add to all swagger documents
                        if (actionApiVersionModel?.IsApiVersionNeutral != false)
                        {
                            return true;
                        }

                        if (actionApiVersionModel.DeclaredApiVersions.Count > 0)
                        {
                            return actionApiVersionModel.DeclaredApiVersions.Any(v => $"v{v}" == docName);
                        }

                        return actionApiVersionModel.ImplementedApiVersions.Any(v => $"v{v}" == docName);
                    });
                });

            return services;
        }

        public static IApplicationBuilder ApplySwaggerMiddleware(this IApplicationBuilder app)
        {
            return app
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    const string Title = "SolutionTemplate API";
                    c.SwaggerEndpoint($"/swagger/v1.0/swagger.json", $"{Title} - v1.0");
                    c.EnableDeepLinking();
                    c.DisplayRequestDuration();
                });
        }
    }

    public class HttpHeadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo.GetCustomAttributes(typeof(HttpHeadAttribute), false)?.Length > 0)
            {
                foreach (var response in operation.Responses.Values)
                {
                    response.Content = null;
                }
            }
        }
    }

    public static class ActionDescriptorExtensions
    {
        public static ApiVersionModel? GetApiVersion(this ActionDescriptor actionDescriptor) =>
            actionDescriptor?.Properties
                .Where(kvp => ((Type)kvp.Key) == typeof(ApiVersionModel))
                .Select(kvp => kvp.Value as ApiVersionModel)
                .FirstOrDefault();
    }
}
