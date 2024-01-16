using System.Security.Claims;

namespace Prohelika.Template.CleanArchitecture.Presentation.Common.Extensions;

/// <summary>
/// Extension on user/ClaimsPrincipal
/// </summary>
public static class UserExtension
{
    /// <summary>
    /// Get the user id
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static string? GetId(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    /// <summary>
    /// Get the user name
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static string? GetUserName(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Name)?.Value;
    }

    /// <summary>
    /// Get email
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static string? GetEmail(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Email)?.Value;
    }
}