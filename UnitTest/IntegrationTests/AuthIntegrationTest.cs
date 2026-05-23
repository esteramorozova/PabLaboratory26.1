using System.Net;
using System.Net.Http.Json;
using AppCore.Dto;

namespace UnitTest.IntegrationTests;

public class AuthIntegrationTest : IClassFixture<WebAppFactory>
{
    private readonly HttpClient _client;

    public AuthIntegrationTest(WebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_ValidCredentials_Returns200()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new LoginDto
        {
            Email = "admin@wsei.edu.pl",
            Password = "Admin123!"
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Login_InvalidCredentials_Returns401()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new LoginDto
        {
            Email = "admin@wsei.edu.pl",
            Password = "zle_haslo"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetContacts_WithoutToken_Returns401()
    {
        var response = await _client.GetAsync("/api/contacts");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetContacts_WithValidToken_Returns200()
    {
        // Logowanie
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new LoginDto
        {
            Email = "admin@wsei.edu.pl",
            Password = "Admin123!"
        });

        var authResult = await loginResponse.Content.ReadFromJsonAsync<AuthResponseDto>();
        
        // Żądanie z tokenem
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authResult!.AccessToken);

        var response = await _client.GetAsync("/api/contacts");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}