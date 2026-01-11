using DroneDelivery.Mobile.Helpers;
using DroneDelivery.Mobile.Models;

namespace DroneDelivery.Mobile.Services;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(string email, string password);
    Task LogoutAsync();
    Task<bool> IsLoggedInAsync();
}

public class AuthService : IAuthService
{
    private readonly IApiClient _api;
    private readonly ITokenStore _tokenStore;

    public AuthService(IApiClient api, ITokenStore tokenStore)
    {
        _api = api;
        _tokenStore = tokenStore;
    }

    public async Task<AuthResponse> LoginAsync(string email, string password)
    {
        // In demo/mock, se accepta orice email/parola ne-goale.
        var response = await _api.PostAsync<AuthResponse>(ApiEndpoints.AuthLogin, new LoginRequest
        {
            Email = email,
            Password = password
        });

        if (string.IsNullOrWhiteSpace(response.Token))
            throw new InvalidOperationException("Login failed: token is empty.");

        await _tokenStore.SaveTokenAsync(response.Token);
        return response;
    }

    public async Task LogoutAsync()
    {
        await _tokenStore.ClearAsync();
    }

    public async Task<bool> IsLoggedInAsync()
    {
        var token = await _tokenStore.GetTokenAsync();
        return !string.IsNullOrWhiteSpace(token);
    }
}
