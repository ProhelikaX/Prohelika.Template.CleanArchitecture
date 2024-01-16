using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;
using Prohelika.Template.CleanArchitecture.Presentation.Common.Utils;

namespace Prohelika.Template.CleanArchitecture.Presentation.WebCSR.Utils;

public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public CustomAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation,
        IOptions<ApiConfigurationOptions> apiConfigOptions) : base(
        provider, navigation)
    {
        ConfigureHandler(authorizedUrls: new[] { apiConfigOptions.Value.BaseUrl },
            scopes: new[] { "openid", "profile", "email", "offline_access" });
    }
}