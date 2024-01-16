namespace Prohelika.Template.CleanArchitecture.Presentation.Common.Utils;

public class OpenIdConnectConfigurationOptions
{
    public const string Key = "OpenIdConnect";

    public string Authority { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string TokenEndpoint { get; set; }
}