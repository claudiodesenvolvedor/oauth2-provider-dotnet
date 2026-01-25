using System.Net;
using System.Text.Json;
using OAuth2.Tests.Infrastructure;

namespace OAuth2.Tests.Token;

public sealed class ClientCredentialsTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    public ClientCredentialsTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Token_ValidClientCredentials_ReturnsToken()
    {
        var response = await _client.PostAsync(
            "/oauth/token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["client_id"] = "test-client",
                ["client_secret"] = "test-secret",
                ["scope"] = "api.read"
            }));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        using var json = JsonDocument.Parse(content);
        var accessToken = json.RootElement.GetProperty("accessToken").GetString();

        Assert.False(string.IsNullOrWhiteSpace(accessToken));
    }

    [Fact]
    public async Task Token_InvalidClientSecret_ReturnsBadRequest()
    {
        var response = await _client.PostAsync(
            "/oauth/token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["client_id"] = "test-client",
                ["client_secret"] = "wrong-secret",
                ["scope"] = "api.read"
            }));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Token_InvalidScope_ReturnsBadRequest()
    {
        var response = await _client.PostAsync(
            "/oauth/token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["client_id"] = "test-client",
                ["client_secret"] = "test-secret",
                ["scope"] = "api.write"
            }));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Token_InvalidGrantType_ReturnsBadRequest()
    {
        var response = await _client.PostAsync(
            "/oauth/token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "password",
                ["client_id"] = "test-client",
                ["client_secret"] = "test-secret"
            }));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
