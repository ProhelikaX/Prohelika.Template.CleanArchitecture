using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Prohelika.Template.CleanArchitecture.Presentation.WebCSR.Utils;

using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;

public class CustomUserFactory
    : AccountClaimsPrincipalFactory<CustomUserAccount>
{
    private readonly IAccessTokenProviderAccessor _accessor;

    public CustomUserFactory(NavigationManager navigation,
        IAccessTokenProviderAccessor accessor) : base(accessor)
    {
        _accessor = accessor;
    }


    public override async ValueTask<ClaimsPrincipal> CreateUserAsync(CustomUserAccount account,
        RemoteAuthenticationUserOptions options)
    {
        var initialUser = await base.CreateUserAsync(account, options);

        if (!(initialUser.Identity?.IsAuthenticated ?? false))
        {
            return initialUser;
        }

        var userIdentity = (ClaimsIdentity)initialUser.Identity;

        var accessTokenResult = await _accessor.TokenProvider.RequestAccessToken();

        if (!accessTokenResult.TryGetToken(out var token))
        {
            return initialUser;
        }

        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token.Value);

        var realmAccessValue = jwtSecurityToken.Payload["realm_access"].ToString();
        if (string.IsNullOrWhiteSpace(realmAccessValue)) return initialUser;
        using var realmAccess = JsonDocument.Parse(realmAccessValue);
        var realmRoles = realmAccess
            .RootElement
            .GetProperty("roles");


        foreach (var value in realmRoles.EnumerateArray().Select(role => role.GetString())
                     .Where(value => !string.IsNullOrWhiteSpace(value)))
        {
            if (value != null) userIdentity.AddClaim(new Claim(ClaimTypes.Role, value));
        }

        return initialUser;
    }
}

public class CustomUserAccount : RemoteUserAccount
{
    [JsonPropertyName("groups")] public string[] Groups { get; set; } = default!;

    [JsonPropertyName("roles")] public string[] Roles { get; set; } = default!;
}