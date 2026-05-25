using System.Net;
using System.Net.Http.Json;
using AppCore.Dto;

namespace UnitTest.IntegrationTests;

public class AuthIntegrationTest : IClassFixture<WebAppFactory>
{
    private readonly HttpClient _client;
    
    // Użytkownik z IdentityDbSeeder — tworzony przez UserManager z poprawnym hashem
    private const string TestEmail = "admin@crm.pl";
    private const string TestPassword = "Admin@123!";

    public AuthIntegrationTest(WebAppFactory factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task Login_ValidCredentials_Returns200()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new LoginDto
        {
            Email = TestEmail,
            Password = TestPassword
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Login_InvalidCredentials_Returns401()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new LoginDto
        {
            Email = TestEmail,
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
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new LoginDto
        {
            Email = TestEmail,
            Password = TestPassword
        });

        var authResult = await loginResponse.Content.ReadFromJsonAsync<AuthResponseDto>();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authResult!.AccessToken);

        var response = await _client.GetAsync("/api/contacts");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}