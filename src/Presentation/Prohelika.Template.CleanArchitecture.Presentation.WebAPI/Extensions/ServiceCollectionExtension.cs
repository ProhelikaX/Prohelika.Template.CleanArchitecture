using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Prohelika.Template.CleanArchitecture.Application.Extensions;
using Prohelika.Template.CleanArchitecture.Infrastructure.Extensions;
using Prohelika.Template.CleanArchitecture.Presentation.Common.Utils;
using Prohelika.Template.CleanArchitecture.Presentation.WebAPI.Middlewares;

namespace Prohelika.Template.CleanArchitecture.Presentation.WebAPI.Extensions;

/// <summary>
///    This class is used to register all services for API project.
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    ///   Configure application services
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var appOptions = configuration.GetSection(AppConfigurationOptions.Key).Get<AppConfigurationOptions>();

        var jwtOptions = configuration.GetSection(JwtConfigurationOptions.Key).Get<JwtConfigurationOptions>();

        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddHttpContextAccessor();

        services.AddHealthChecks();

        services.AddSwagger(appOptions, jwtOptions);

        services.AddScoped<LoggerMiddleware>();

        services.AddScoped<ExceptionMiddleware>();

        services.AddApplication();
        
        services.AddInfrastructure(configuration);

        services.AddCors(options =>
        {
            options.AddPolicy(AppCorsPolicy.AllowAll, builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        if (jwtOptions!.Keycloak)
        {
            services.AddTransient<IClaimsTransformation, KeycloakClaimsTransformation>();
        }

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.MetadataAddress = jwtOptions.MetadataAddress;
            x.RequireHttpsMetadata = false; // only for dev
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                // ValidIssuer = jwtOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtOptions.Keycloak ? "account" : jwtOptions.Audience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.FromMinutes(1)
            };
        });

        services.AddAuthorization(o =>
        {
            if (jwtOptions.Keycloak)
            {
                o.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim("email_verified", "true")
                    .Build();
            }
            else
            {
                o.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            }

            o.AddPolicy(AppAuthPolicy.IsAdmin,
                policy => { policy.RequireAssertion(context => context.User.IsInRole(AppAuthRole.Admin)); });

            o.AddPolicy(AppAuthPolicy.IsSuperAdmin,
                policy => { policy.RequireAssertion(context => context.User.IsInRole(AppAuthRole.Superadmin)); });

            o.AddPolicy(AppAuthPolicy.IsSuperAdminOrAdmin,
                policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.IsInRole(AppAuthRole.Superadmin) || context.User.IsInRole(AppAuthRole.Admin));
                });
        });

        return services;
    }
}