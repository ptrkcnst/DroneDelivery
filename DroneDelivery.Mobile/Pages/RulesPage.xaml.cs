using DroneDelivery.Mobile.ViewModels;

namespace DroneDelivery.Mobile.Pages;

public partial class RulesPage : ContentPage
{
    private readonly RulesViewModel _vm;

    public RulesPage() : this(GetRequiredService<RulesViewModel>())
    {
    }

    public RulesPage(RulesViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadAsync();
    }

    private static T GetRequiredService<T>() where T : class
    {
        var services = Application.Current?.Handler?.MauiContext?.Services;
        return services?.GetService(typeof(T)) as T
               ?? throw new InvalidOperationException($"{typeof(T).Name} not registered in DI");
    }
}
