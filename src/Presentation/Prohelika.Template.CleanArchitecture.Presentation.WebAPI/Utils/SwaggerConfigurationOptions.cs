namespace Prohelika.Template.CleanArchitecture.Presentation.WebAPI.Utils;

/// <summary>
/// Swagger configuration
/// </summary>
public class SwaggerConfigurationOptions
{
    public const string Key = "Swagger";
    public string OAuthClientId { get; set; }
    public string OAuthClientSecret { get; set; }
    public string OAuthScopes { get; set; }
}