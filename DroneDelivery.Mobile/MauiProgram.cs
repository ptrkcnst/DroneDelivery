using DroneDelivery.Mobile.Services;
using DroneDelivery.Mobile.ViewModels;
using DroneDelivery.Mobile.Pages;

namespace DroneDelivery.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Core services
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<App>();
        builder.Services.AddSingleton<HttpClient>();
        // Inregistreaza si mock si real, iar SwitchableApiClient decide la runtime.
        builder.Services.AddSingleton<ApiClient>();
        builder.Services.AddSingleton<MockApiClient>();
        builder.Services.AddSingleton<IApiClient, SwitchableApiClient>();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<ITokenStore, TokenStore>();

        // Domain services
        builder.Services.AddSingleton<IAddressService, AddressService>();
        builder.Services.AddSingleton<IOrderService, OrderService>();
        builder.Services.AddSingleton<IRuleService, RuleService>();
        builder.Services.AddSingleton<INotificationService, NotificationService>();

        // ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<DeliveryRequestViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();

        // Pages
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<DeliveryRequestPage>();
        builder.Services.AddTransient<SettingsPage>();

        return builder.Build();
    }
}
