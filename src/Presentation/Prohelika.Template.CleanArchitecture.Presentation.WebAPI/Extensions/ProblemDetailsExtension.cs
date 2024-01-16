using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Prohelika.Template.CleanArchitecture.Presentation.WebAPI.Extensions;

/// <summary>
/// Extension on ProblemDetails
/// </summary>
public static class ProblemDetailsExtension
{
    /// <summary>
    /// Serialize problem details
    /// </summary>
    /// <param name="problemDetails"></param>
    /// <returns></returns>
    public static string ToJson(this ProblemDetails problemDetails)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return JsonSerializer.Serialize(problemDetails, options);
    }
}