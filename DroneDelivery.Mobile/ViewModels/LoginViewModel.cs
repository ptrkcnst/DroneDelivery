using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DroneDelivery.Mobile.Services;
using Microsoft.Maui.Storage;

namespace DroneDelivery.Mobile.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private readonly IAuthService _auth;

    [ObservableProperty] private string email = "";
    [ObservableProperty] private string password = "";

    public LoginViewModel(IAuthService auth)
    {
        _auth = auth;
        Title = "Login";
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        Error = "";
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            Error = "Email și parola sunt obligatorii.";
            return;
        }

        try
        {
            IsBusy = true;
            await _auth.LoginAsync(Email, Password);
            Preferences.Set("LastLoginEmail", Email);
            await Shell.Current.GoToAsync("//schedule");
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
