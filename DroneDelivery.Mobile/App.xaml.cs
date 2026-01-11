using DroneDelivery.Mobile.Helpers;
using DroneDelivery.Mobile.Services;
using Microsoft.Maui.Storage;

namespace DroneDelivery.Mobile;

public partial class App : Application
{
    private const string ApiBaseUrlKey = "ApiBaseUrl";
    private const string DefaultApiBaseUrl = "http://10.0.2.2:5001";

    public App(AppShell shell)
    {
        InitializeComponent();
        MainPage = shell;
    }

    protected override void OnStart()
    {
        base.OnStart();

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                if (!Preferences.ContainsKey("UseMockBackend"))
                {
                    SwitchableApiClient.UseMock = false;
                }
                if (!Preferences.ContainsKey(ApiBaseUrlKey))
                {
                    ApiEndpoints.BaseUrl = DefaultApiBaseUrl;
                }
                else if (string.Equals(ApiEndpoints.BaseUrl, "http://10.0.2.2:5000", StringComparison.OrdinalIgnoreCase))
                {
                    ApiEndpoints.BaseUrl = DefaultApiBaseUrl;
                }

                // Shell poate fi null pentru o fracțiune de timp
                if (Shell.Current is null)
                    return;

                await Shell.Current.GoToAsync("//login");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        });
    }
}
