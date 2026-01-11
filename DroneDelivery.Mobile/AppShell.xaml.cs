using DroneDelivery.Mobile.Pages;

namespace DroneDelivery.Mobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Rute (navigare prin GoToAsync)
        Routing.RegisterRoute("login", typeof(LoginPage));
        Routing.RegisterRoute("schedule", typeof(DeliveryRequestPage));

        Routing.RegisterRoute("settings", typeof(SettingsPage));

        // Rute pentru paginile de editare (folosite cu nameof(Pages.*))
        Routing.RegisterRoute(nameof(AddressEditPage), typeof(AddressEditPage));
        Routing.RegisterRoute(nameof(OrderEditPage), typeof(OrderEditPage));
        Routing.RegisterRoute(nameof(RuleEditPage), typeof(RuleEditPage));
    }
}
