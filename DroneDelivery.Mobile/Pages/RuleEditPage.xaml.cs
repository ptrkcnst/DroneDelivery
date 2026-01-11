using DroneDelivery.Mobile.ViewModels;

namespace DroneDelivery.Mobile.Pages;

public partial class RuleEditPage : ContentPage
{
    public RuleEditPage() : this(GetRequiredService<RuleEditViewModel>())
    {
    }

    public RuleEditPage(RuleEditViewModel vm)
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
