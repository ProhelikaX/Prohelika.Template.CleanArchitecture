using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Prohelika.Template.CleanArchitecture.Presentation.Common.Utils;
using Prohelika.Template.CleanArchitecture.Presentation.WebSSR.Utils;

namespace Prohelika.Template.CleanArchitecture.Presentation.WebSSR.Extensions;

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
        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        services.AddHttpContextAccessor();

        services.AddScoped<CustomAuthorizationMessageHandler>();

        var apiOptions = configuration.GetSection(ApiConfigurationOptions.Key).Get<ApiConfigurationOptions>();

        services.AddHttpClient("api",
                client => client.BaseAddress = new Uri(apiOptions!.BaseUrl))
            .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

        services.AddScoped(
            sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("api"));

        services.AddHealthChecks();

        var openIdConnectOptions =
            configuration.GetSection(OpenIdConnectConfigurationOptions.Key).Get<OpenIdConnectConfigurationOptions>();

        services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme =
                        CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme =
                        CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme =
                        OpenIdConnectDefaults.AuthenticationScheme;
                }
            )
            .AddOpenIdConnect(
                options =>
                {
                    options.Authority = openIdConnectOptions?.Authority;
                    options.ClientId = openIdConnectOptions?.ClientId;
                    options.ClientSecret = openIdConnectOptions?.ClientSecret;
                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.UsePkce = true;
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("email");
                    options.Scope.Add("offline_access");
                    options.Scope.Add("roles");
                    options.SaveTokens = true;
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.UseTokenLifetime = true;
                    options.RefreshInterval = new TimeSpan(0, 0, 30);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "preferred_username",
                    };

                    options.Events = new OpenIdConnectEvents
                    {
                        OnTokenValidated = ctx =>
                        {
                            var identity = ctx.Principal?.Identities.First();

                            var token = ctx.TokenEndpointResponse?.AccessToken;

                            var tokenHandler = new JwtSecurityTokenHandler();
                            var securityToken = tokenHandler.ReadJwtToken(token);


                            var realmAccessValue = securityToken.Claims.FirstOrDefault(c => c.Type == "realm_access")
                                ?.Value;

                            if (string.IsNullOrWhiteSpace(realmAccessValue))
                            {
                                return Task.CompletedTask;
                            }


                            var realmAccessParsed = JsonDocument.Parse(realmAccessValue);


                            var realmRoles = realmAccessParsed
                                .RootElement
                                .GetProperty("roles");

                            foreach (var value in realmRoles.EnumerateArray().Select(role => role.GetString())
                                         .Where(value => !string.IsNullOrWhiteSpace(value)))
                            {
                                if (value == null) continue;
                                identity?.AddClaim(new Claim(ClaimTypes.Role, value));
                            }


                            if (identity != null) ctx.Principal?.AddIdentity(identity);

                            return Task.CompletedTask;
                        },
                    };
                }).AddCookie(options =>
            {
                options.Events = new CookieAuthenticationEvents
                {
                    // OnValidatePrincipal = context =>
                    // {
                    //     //check to see if user is authenticated first
                    //     if (context.Principal?.Identity?.IsAuthenticated != true) return Task.CompletedTask;
                    //     //get the user's tokens
                    //     var tokens = context.Properties.GetTokens().ToList();
                    //     var refreshToken = tokens.FirstOrDefault(t => t.Name == "refresh_token");
                    //     var accessToken = tokens.FirstOrDefault(t => t.Name == "access_token");
                    //     foreach (var token in tokens)
                    //     {
                    //         Console.WriteLine(token.Name + ": " + token.Value);
                    //     }
                    //     var exp = tokens.FirstOrDefault(t => t.Name == "expires_at");
                    //     var expires = DateTime.Parse(exp.Value);
                    //     //check to see if the token has expired
                    //     if (expires >= DateTime.Now) return Task.CompletedTask;
                    //     //token is expired, let's attempt to renew
                    //     const string tokenEndpoint =
                    //         "https://auth.prohelika.net/realms/Test/protocol/openid-connect/token";
                    //     var tokenClient = new TokenClient(tokenEndpoint, "web", "v9yK9QPBtGCLeeT0aog16XuNogwekCj2");
                    //     try
                    //     {
                    //         var tokenResponse = tokenClient.RequestRefreshTokenAsync(refreshToken?.Value).Result;
                    //         //check for error while renewing - any error will trigger a new login.
                    //
                    //         //set new token values
                    //         if (refreshToken != null) refreshToken.Value = tokenResponse.RefreshToken;
                    //         if (accessToken != null) accessToken.Value = tokenResponse.AccessToken;
                    //         //set new expiration date
                    //         var newExpires = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResponse.ExpiresIn);
                    //         exp.Value = newExpires.ToString("o", CultureInfo.InvariantCulture);
                    //         //set tokens in auth properties 
                    //         context.Properties.StoreTokens(tokens);
                    //         //trigger context to renew cookie with new token values
                    //         context.ShouldRenew = true;
                    //     }
                    //     catch (Exception e)
                    //     {
                    //         //reject Principal
                    //         context.RejectPrincipal();
                    //     }
                    //
                    //     return Task.CompletedTask;
                    // }
                };
            });

        services.AddAuthorizationBuilder()
            .SetDefaultPolicy(new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("email_verified", "true")
                .Build())
            .AddPolicy(AppAuthPolicy.IsAdmin,
                policy => { policy.RequireAssertion(context => context.User.IsInRole(AppAuthRole.Admin)); })
            .AddPolicy(AppAuthPolicy.IsSuperAdmin,
                policy => { policy.RequireAssertion(context => context.User.IsInRole(AppAuthRole.Superadmin)); })
            .AddPolicy(AppAuthPolicy.IsSuperAdminOrAdmin, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.IsInRole(AppAuthRole.Superadmin) || context.User.IsInRole(AppAuthRole.Admin));
            });


        return services;
    }
}