using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace Prohelika.Template.CleanArchitecture.Presentation.WebSSR.Utils;

public class CustomAuthorizationMessageHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var context = httpContextAccessor.HttpContext;

        if (context == null) return await base.SendAsync(request, cancellationToken);

        var accessToken = await context.GetTokenAsync("access_token");
        var refreshToken = await context.GetTokenAsync("refresh_token");

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}