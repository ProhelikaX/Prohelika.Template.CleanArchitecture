using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Prohelika.Template.CleanArchitecture.Presentation.Common.Utils;
using Prohelika.Template.CleanArchitecture.Presentation.WebCSR.Utils;

namespace Prohelika.Template.CleanArchitecture.Presentation.WebCSR.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApiConfigurationOptions>(configuration.GetSection(ApiConfigurationOptions.Key));

        var apiOptions = configuration.GetSection(ApiConfigurationOptions.Key).Get<ApiConfigurationOptions>();

        services.AddScoped<CustomAuthorizationMessageHandler>();

        services.AddHttpClient(ApiConfigurationOptions.Key,
                client => client.BaseAddress = new Uri(apiOptions!.BaseUrl))
            .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();


        services.AddScoped(
            sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient(ApiConfigurationOptions.Key));

        services.AddOidcAuthentication<RemoteAuthenticationState, CustomUserAccount>(options =>
            {
                configuration.Bind("Local", options.ProviderOptions);

                options.ProviderOptions.ResponseType = "code";
                options.ProviderOptions.DefaultScopes.Add("openid");
                options.ProviderOptions.DefaultScopes.Add("profile");
                options.ProviderOptions.DefaultScopes.Add("email");
                options.ProviderOptions.DefaultScopes.Add("offline_access");

                options.UserOptions.NameClaim = "preferred_username";
                options.UserOptions.RoleClaim = ClaimTypes.Role;
                options.UserOptions.ScopeClaim = "scope";
            })
            .AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, CustomUserAccount, CustomUserFactory>();

        services.AddApiAuthorization();
        services.AddAuthorizationCore(
            options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    // .RequireClaim("email_verified", "true")
                    .Build();
                options.AddPolicy(AppAuthPolicy.IsAdmin,
                    policy => { policy.RequireAssertion(context => context.User.IsInRole(AppAuthRole.Admin)); });
                options.AddPolicy(AppAuthPolicy.IsSuperAdmin,
                    policy => { policy.RequireAssertion(context => context.User.IsInRole(AppAuthRole.Superadmin)); });
                options.AddPolicy(AppAuthPolicy.IsSuperAdminOrAdmin, policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.IsInRole(AppAuthRole.Superadmin) || context.User.IsInRole(AppAuthRole.Admin));
                });
            });


        return services;
    }
}