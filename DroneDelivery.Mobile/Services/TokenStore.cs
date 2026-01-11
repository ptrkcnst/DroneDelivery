namespace DroneDelivery.Mobile.Services;

public interface ITokenStore
{
    Task SaveTokenAsync(string token);
    Task<string?> GetTokenAsync();
    Task ClearAsync();
}

public class TokenStore : ITokenStore
{
    private const string Key = "auth_token";

    public async Task SaveTokenAsync(string token)
        => await SecureStorage.SetAsync(Key, token);

    public async Task<string?> GetTokenAsync()
        => await SecureStorage.GetAsync(Key);

    public Task ClearAsync()
    {
        SecureStorage.Remove(Key);
        return Task.CompletedTask;
    }
}
