using System.Runtime.Serialization;

namespace Prohelika.Template.CleanArchitecture.Presentation.Common.Utils;

/// <summary>
/// Authentication related options
/// </summary>
[DataContract]
public class JwtConfigurationOptions
{
    public const string Key = "Jwt";
    public string MetadataAddress { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public bool Keycloak { get; set; }
}