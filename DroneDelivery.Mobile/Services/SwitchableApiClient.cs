using Microsoft.Maui.Storage;

namespace DroneDelivery.Mobile.Services;

/// <summary>
/// Alege dinamic intre backend real si mock, pe baza preferintei "UseMockBackend".
/// Astfel aplicatia poate porni fara API, dar poti comuta pe URL real fara rebuild.
/// </summary>
public class SwitchableApiClient : IApiClient
{
    private const string UseMockKey = "UseMockBackend";

    private readonly ApiClient _real;
    private readonly MockApiClient _mock;

    public SwitchableApiClient(ApiClient real, MockApiClient mock)
    {
        _real = real;
        _mock = mock;
    }

    public static bool UseMock
    {
        get => Preferences.Get(UseMockKey, false);
        set => Preferences.Set(UseMockKey, value);
    }

    private IApiClient Current => UseMock ? _mock : _real;

    public Task<T> GetAsync<T>(string path) => Current.GetAsync<T>(path);

    public Task<T> PostAsync<T>(string path, object body) => Current.PostAsync<T>(path, body);

    public Task PutAsync(string path, object body) => Current.PutAsync(path, body);

    public Task DeleteAsync(string path) => Current.DeleteAsync(path);

    public Task SetBearerTokenAsync() => Current.SetBearerTokenAsync();
}
