using System.Text.Json;
using System.Text.Json.Serialization;

namespace Prohelika.Template.CleanArchitecture.Presentation.WebSSR.Extensions;

public class TokenClient(string tokenEndpoint, string clientId, string clientSecret)
{
    public async Task<TokenResponse?> RequestRefreshTokenAsync(string? refreshToken)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);
        var collection = new List<KeyValuePair<string, string>>
        {
            new("client_id", clientId),
            new("client_secret", clientSecret),
            new("grant_type", "refresh_token"),
            new("refresh_token", refreshToken)
        };
        var content = new FormUrlEncodedContent(collection);
        request.Content = content;
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<TokenResponse>(await response.Content.ReadAsStringAsync());
    }
}

public class TokenResponse
{
    [JsonPropertyName("access_token")] public string AccessToken { get; set; }

    [JsonPropertyName("expires_in")] public long ExpiresIn { get; set; }

    [JsonPropertyName("refresh_expires_in")]
    public long RefreshExpiresIn { get; set; }

    [JsonPropertyName("refresh_token")] public string RefreshToken { get; set; }

    [JsonPropertyName("token_type")] public string TokenType { get; set; }

    [JsonPropertyName("not-before-policy")]
    public long NotBeforePolicy { get; set; }

    [JsonPropertyName("session_state")] public Guid SessionState { get; set; }

    [JsonPropertyName("scope")] public string Scope { get; set; }
}