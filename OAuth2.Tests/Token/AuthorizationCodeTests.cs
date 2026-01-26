using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using System.Web;
using OAuth2.Tests.Infrastructure;

namespace OAuth2.Tests.Token;

public sealed class AuthorizationCodeTests : IClassFixture<ApiFactory>
{
    private const string RedirectUri = "https://example.com/callback";
    private const string CodeVerifier = "test-code-verifier-1234567890";
    private readonly HttpClient _client;

    public AuthorizationCodeTests(ApiFactory factory)
    {
        _client = factory.CreateClient(new() { AllowAutoRedirect = false });
    }

    [Fact]
    public async Task Authorize_ReturnsRedirectWithCode()
    {
        var codeChallenge = ComputeCodeChallenge(CodeVerifier);
        var response = await _client.GetAsync(
            $"/oauth/authorize?client_id=test-client&redirect_uri={Uri.EscapeDataString(RedirectUri)}&scope=api.read&state=abc&code_challenge={codeChallenge}&code_challenge_method=S256");

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.NotNull(response.Headers.Location);

        var query = HttpUtility.ParseQueryString(response.Headers.Location!.Query);
        var code = query["code"];

        Assert.False(string.IsNullOrWhiteSpace(code));
    }

    [Fact]
    public async Task Token_ExchangeAuthorizationCode_ReturnsToken()
    {
        var code = await GetAuthorizationCodeAsync();

        var response = await _client.PostAsync(
            "/oauth/token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["client_id"] = "test-client",
                ["client_secret"] = "test-secret",
                ["code"] = code,
                ["redirect_uri"] = RedirectUri,
                ["code_verifier"] = CodeVerifier
            }));

        var body = await response.Content.ReadAsStringAsync();
        Assert.True(
            response.StatusCode == HttpStatusCode.OK,
            $"Unexpected status: {response.StatusCode}. Body: {body}");

        using var json = JsonDocument.Parse(body);
        var accessToken = json.RootElement.GetProperty("accessToken").GetString();

        Assert.False(string.IsNullOrWhiteSpace(accessToken));
    }

    [Fact]
    public async Task Token_ReusingAuthorizationCode_ReturnsBadRequest()
    {
        var code = await GetAuthorizationCodeAsync();

        var response = await _client.PostAsync(
            "/oauth/token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["client_id"] = "test-client",
                ["client_secret"] = "test-secret",
                ["code"] = code,
                ["redirect_uri"] = RedirectUri,
                ["code_verifier"] = CodeVerifier
            }));

        var body = await response.Content.ReadAsStringAsync();
        Assert.True(
            response.StatusCode == HttpStatusCode.OK,
            $"Unexpected status: {response.StatusCode}. Body: {body}");

        var reuseResponse = await _client.PostAsync(
            "/oauth/token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["client_id"] = "test-client",
                ["client_secret"] = "test-secret",
                ["code"] = code,
                ["redirect_uri"] = RedirectUri,
                ["code_verifier"] = CodeVerifier
            }));

        var reuseBody = await reuseResponse.Content.ReadAsStringAsync();
        Assert.True(
            reuseResponse.StatusCode == HttpStatusCode.BadRequest,
            $"Unexpected status: {reuseResponse.StatusCode}. Body: {reuseBody}");
    }

    private async Task<string> GetAuthorizationCodeAsync()
    {
        var codeChallenge = ComputeCodeChallenge(CodeVerifier);
        var response = await _client.GetAsync(
            $"/oauth/authorize?client_id=test-client&redirect_uri={Uri.EscapeDataString(RedirectUri)}&scope=api.read&state=abc&code_challenge={codeChallenge}&code_challenge_method=S256");

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.NotNull(response.Headers.Location);

        var query = HttpUtility.ParseQueryString(response.Headers.Location!.Query);
        var code = query["code"];

        Assert.False(string.IsNullOrWhiteSpace(code));
        return code!;
    }

    [Fact]
    public async Task Token_MissingVerifier_ReturnsBadRequest()
    {
        var code = await GetAuthorizationCodeAsync();

        var response = await _client.PostAsync(
            "/oauth/token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["client_id"] = "test-client",
                ["client_secret"] = "test-secret",
                ["code"] = code,
                ["redirect_uri"] = RedirectUri
            }));

        var body = await response.Content.ReadAsStringAsync();
        Assert.True(
            response.StatusCode == HttpStatusCode.BadRequest,
            $"Unexpected status: {response.StatusCode}. Body: {body}");
    }

    [Fact]
    public async Task Token_InvalidVerifier_ReturnsBadRequest()
    {
        var code = await GetAuthorizationCodeAsync();

        var response = await _client.PostAsync(
            "/oauth/token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["client_id"] = "test-client",
                ["client_secret"] = "test-secret",
                ["code"] = code,
                ["redirect_uri"] = RedirectUri,
                ["code_verifier"] = "wrong-verifier"
            }));

        var body = await response.Content.ReadAsStringAsync();
        Assert.True(
            response.StatusCode == HttpStatusCode.BadRequest,
            $"Unexpected status: {response.StatusCode}. Body: {body}");
    }

    private static string ComputeCodeChallenge(string verifier)
    {
        var bytes = Encoding.ASCII.GetBytes(verifier);
        var hash = SHA256.HashData(bytes);
        return Base64UrlEncode(hash);
    }

    private static string Base64UrlEncode(byte[] data)
    {
        return Convert.ToBase64String(data)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
}
