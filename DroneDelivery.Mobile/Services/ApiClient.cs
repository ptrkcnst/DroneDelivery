using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DroneDelivery.Mobile.Helpers;

namespace DroneDelivery.Mobile.Services;

public class ApiException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string? ResponseBody { get; }

    public ApiException(string message, HttpStatusCode statusCode, string? body = null) : base(message)
    {
        StatusCode = statusCode;
        ResponseBody = body;
    }
}

public interface IApiClient
{
    Task<T> GetAsync<T>(string path);
    Task<T> PostAsync<T>(string path, object body);
    Task PutAsync(string path, object body);
    Task DeleteAsync(string path);
    Task SetBearerTokenAsync();
}

/// <summary>
/// Client HTTP "real". Cand backend-ul nu e pornit, vei primi erori de retea (capturable in VM).
/// Pentru a rula aplicatia fara backend, in MauiProgram se inregistreaza in DEBUG un MockApiClient.
/// </summary>
public class ApiClient : IApiClient
{
    private readonly HttpClient _http;
    private readonly ITokenStore _tokenStore;

    public ApiClient(HttpClient http, ITokenStore tokenStore)
    {
        _http = http;
        _tokenStore = tokenStore;
    }

    private static Uri BuildUri(string path)
    {
        // path in proiect este de forma "/api/...".
        return new Uri($"{ApiEndpoints.BaseUrl}{path}");
    }

    public async Task SetBearerTokenAsync()
    {
        var token = await _tokenStore.GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization = null;
            return;
        }

        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<T> GetAsync<T>(string path)
    {
        await SetBearerTokenAsync();
        using var resp = await _http.GetAsync(BuildUri(path));
        return await ReadAsync<T>(resp);
    }

    public async Task<T> PostAsync<T>(string path, object body)
    {
        await SetBearerTokenAsync();
        using var resp = await _http.PostAsJsonAsync(BuildUri(path), body);
        return await ReadAsync<T>(resp);
    }

    public async Task PutAsync(string path, object body)
    {
        await SetBearerTokenAsync();
        using var resp = await _http.PutAsJsonAsync(BuildUri(path), body);
        if (!resp.IsSuccessStatusCode)
        {
            var text = await resp.Content.ReadAsStringAsync();
            throw new ApiException($"PUT {path} failed", resp.StatusCode, text);
        }
    }

    public async Task DeleteAsync(string path)
    {
        await SetBearerTokenAsync();
        using var resp = await _http.DeleteAsync(BuildUri(path));
        if (!resp.IsSuccessStatusCode)
        {
            var text = await resp.Content.ReadAsStringAsync();
            throw new ApiException($"DELETE {path} failed", resp.StatusCode, text);
        }
    }

    private static async Task<T> ReadAsync<T>(HttpResponseMessage resp)
    {
        if (resp.IsSuccessStatusCode)
        {
            var data = await resp.Content.ReadFromJsonAsync<T>();
            if (data is null)
                throw new ApiException("Empty response body", resp.StatusCode);
            return data;
        }

        var text = await resp.Content.ReadAsStringAsync();
        throw new ApiException($"Request failed ({(int)resp.StatusCode})", resp.StatusCode, text);
    }
}
