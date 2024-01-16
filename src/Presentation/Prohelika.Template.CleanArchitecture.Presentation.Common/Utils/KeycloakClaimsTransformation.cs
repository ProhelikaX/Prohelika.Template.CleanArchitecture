using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

namespace Prohelika.Template.CleanArchitecture.Presentation.Common.Utils;

/// <summary>
/// Transforms Keycloak claims into ASP.NET Core claims
/// </summary>
public class KeycloakClaimsTransformation : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var result = principal.Clone();
        if (result.Identity is not ClaimsIdentity identity)
        {
            return Task.FromResult(result);
        }

        foreach (var claim in principal.Claims)
        {
            Console.WriteLine($"""{claim.Type} => {claim.Value}""");
        }

        var nameValue = principal.FindFirst("preferred_username")?.Value;

        if (!string.IsNullOrWhiteSpace(nameValue))
        {
            identity.AddClaim(new Claim(ClaimTypes.Name, nameValue));
        }

        var realmAccessValue = principal.FindFirst("realm_access")?.Value;
        if (string.IsNullOrWhiteSpace(realmAccessValue))
        {
            return Task.FromResult(result);
        }

        using var realmAccess = JsonDocument.Parse(realmAccessValue);
        var realmRoles = realmAccess
            .RootElement
            .GetProperty("roles");


        foreach (var value in realmRoles.EnumerateArray().Select(role => role.GetString())
                     .Where(value => !string.IsNullOrWhiteSpace(value)))
        {
            if (value != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, value));
            }
        }

        return Task.FromResult(principal);
    }
}