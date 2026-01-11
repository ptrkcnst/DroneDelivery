using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DroneDelivery.Mobile.Helpers;
using DroneDelivery.Mobile.Services;

namespace DroneDelivery.Mobile.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    private readonly IAuthService _auth;

    [ObservableProperty]
    private string apiBaseUrl = string.Empty;

    [ObservableProperty]
    private bool useMockBackend;

    public SettingsViewModel(IAuthService auth)
    {
        _auth = auth;
        Title = "Settings";

        ApiBaseUrl = ApiEndpoints.BaseUrl;
        UseMockBackend = SwitchableApiClient.UseMock;
    }

    [RelayCommand]
    private async Task SaveBackendSettingsAsync()
    {
        // Persista setarile.
        ApiEndpoints.BaseUrl = ApiBaseUrl;
        SwitchableApiClient.UseMock = UseMockBackend;

        await Shell.Current.DisplayAlert("Saved", "Backend settings updated.", "OK");
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await _auth.LogoutAsync();
        await Shell.Current.GoToAsync("//login");
    }
}
