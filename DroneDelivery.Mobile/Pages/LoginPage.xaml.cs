using DroneDelivery.Mobile.ViewModels;

namespace DroneDelivery.Mobile.Pages;

public partial class LoginPage : ContentPage
{
    // ShellContent + DataTemplate instantiaza pagina prin ctor fara parametri.
    // Pentru a pasra DI (ViewModel in ctor), oferim un ctor fara parametri
    // care rezolva ViewModel-ul din container.
    public LoginPage() : this(GetRequiredService<LoginViewModel>())
    {
    }

    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private static T GetRequiredService<T>() where T : class
    {
        var services = Application.Current?.Handler?.MauiContext?.Services;
        return services?.GetService(typeof(T)) as T
               ?? throw new InvalidOperationException($"{typeof(T).Name} not registered in DI");
    }
}
