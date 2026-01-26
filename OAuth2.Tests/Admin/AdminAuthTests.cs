using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Net.Http.Json;
using OAuth2.Tests.Infrastructure;

namespace OAuth2.Tests.Admin;

public sealed class AdminAuthTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    public AdminAuthTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Admin_Login_ReturnsAccessToken()
    {
        var response = await _client.PostAsJsonAsync(
            "/auth/login",
            new
            {
                email = "admin@local",
                password = "Admin123!"
            });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        using var json = JsonDocument.Parse(body);
        var accessToken = json.RootElement.GetProperty("accessToken").GetString();

        Assert.False(string.IsNullOrWhiteSpace(accessToken));
    }

    [Fact]
    public async Task Clients_WithoutAuth_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/clients");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Clients_WithAdminToken_ReturnsOk()
    {
        var token = await GetAdminTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/clients");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private async Task<string> GetAdminTokenAsync()
    {
        var response = await _client.PostAsJsonAsync(
            "/auth/login",
            new
            {
                email = "admin@local",
                password = "Admin123!"
            });

        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();
        using var json = JsonDocument.Parse(body);
        var accessToken = json.RootElement.GetProperty("accessToken").GetString();

        Assert.False(string.IsNullOrWhiteSpace(accessToken));
        return accessToken!;
    }
}
