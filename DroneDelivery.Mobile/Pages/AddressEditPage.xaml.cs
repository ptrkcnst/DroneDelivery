using DroneDelivery.Mobile.ViewModels;

namespace DroneDelivery.Mobile.Pages;

public partial class AddressEditPage : ContentPage
{
    public AddressEditPage() : this(GetRequiredService<AddressEditViewModel>())
    {
    }

    public AddressEditPage(AddressEditViewModel vm)
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
