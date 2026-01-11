using CommunityToolkit.Mvvm.ComponentModel;

namespace DroneDelivery.Mobile.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string title = "";
    [ObservableProperty] private string error = "";
}
