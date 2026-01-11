using DroneDelivery.Mobile.ViewModels;

namespace DroneDelivery.Mobile.Pages;

public partial class OrderEditPage : ContentPage
{
    public OrderEditPage() : this(GetRequiredService<OrderEditViewModel>())
    {
    }

    public OrderEditPage(OrderEditViewModel vm)
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
