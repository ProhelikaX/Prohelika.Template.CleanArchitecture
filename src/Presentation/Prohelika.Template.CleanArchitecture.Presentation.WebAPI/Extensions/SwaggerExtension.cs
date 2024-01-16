using System.Reflection;
using Microsoft.OpenApi.Models;
using Prohelika.Template.CleanArchitecture.Presentation.Common.Utils;

namespace Prohelika.Template.CleanArchitecture.Presentation.WebAPI.Extensions;

/// <summary>
/// Extension on Swagger
/// </summary>
public static class SwaggerExtension
{
    /// <summary>
    /// Configure and adds Swagger to the service container
    /// </summary>
    /// <param name="services"></param>
    /// <param name="appOptions"></param>
    /// <param name="jwtOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddSwagger(this IServiceCollection services, AppConfigurationOptions? appOptions,
        JwtConfigurationOptions? jwtOptions)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = appOptions?.AppName ?? "Prohelika.Template.CleanArchitecture Web API",
                Version = appOptions?.AppVersion ?? "1.0.0",
                Description = appOptions?.AppDescription ?? "Clean Architecture template for ASP.NET Core Web API",
                Contact = new OpenApiContact
                {
                    Name = appOptions?.AppAuthor ?? "S. M. JAHANGIR",
                    Url = new Uri(appOptions?.AppAuthorUrl ?? "https://smj.prohelika.org")
                },
                License = new OpenApiLicense
                {
                    Name = appOptions?.AppLicense ?? "MIT",
                    Url = new Uri(appOptions?.AppLicenseUrl ?? "")
                }
            });


            if (jwtOptions != null)
            {
                options.AddSecurityDefinition("OpenId", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OpenIdConnect,
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                    OpenIdConnectUrl = new Uri(jwtOptions.MetadataAddress),
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            Scopes = new Dictionary<string, string>
                            {
                                {
                                    "openid", "openid"
                                },
                                {
                                    "api", "api"
                                },
                            },
                        },
                    },
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "OpenId",
                            },
                        },
                        new List<string> { "openid", "profile", "email", "offline_access", "roles" }
                    },
                });
            }

            var filePath = Path.Combine(AppContext.BaseDirectory,
                Assembly.GetAssembly(typeof(Program))?.GetName().Name + ".xml");
            options.IncludeXmlComments(filePath);
        });

        return services;
    }
}