using Microsoft.Maui.Storage;

namespace DroneDelivery.Mobile.Helpers;

public static class ApiEndpoints
{
    private const string ApiBaseUrlKey = "ApiBaseUrl";

    // Default pentru Android emulator: 10.0.2.2 = localhost-ul masinii host.
    public static string BaseUrl
    {
        get
        {
            var url = Preferences.Get(ApiBaseUrlKey, "http://10.0.2.2:5001");
            url = (url ?? string.Empty).Trim();
            return url.TrimEnd('/');
        }
        set
        {
            var url = (value ?? string.Empty).Trim().TrimEnd('/');
            Preferences.Set(ApiBaseUrlKey, url);
        }
    }

    public const string AuthLogin = "/api/auth/login";
    public const string AuthRegister = "/api/auth/register";

    public const string Addresses = "/api/addresses";
    public const string Orders = "/api/orders";
    public const string NotificationRules = "/api/notification-rules";
    public const string Notifications = "/api/notifications";

    public static string AddressById(int id) => $"{Addresses}/{id}";
    public static string OrderById(int id) => $"{Orders}/{id}";
    public static string RuleById(int id) => $"{NotificationRules}/{id}";
}
