using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using DroneDelivery.Mobile.Models;
using DroneDelivery.Mobile.Services;

namespace DroneDelivery.Mobile.ViewModels;

public partial class NotificationsViewModel : BaseViewModel
{
    private readonly INotificationService _service;
    public ObservableCollection<NotificationDto> Items { get; } = new();

    public NotificationsViewModel(INotificationService service)
    {
        _service = service;
        Title = "Notifications";
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        Error = "";
        try
        {
            IsBusy = true;
            Items.Clear();
            var list = await _service.GetAllAsync();
            foreach (var n in list.OrderByDescending(x => x.SendAt)) Items.Add(n);
        }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task DeleteAsync(NotificationDto dto)
    {
        var ok = await Shell.Current.DisplayAlert("Confirm", $"Ștergi notificarea #{dto.Id}?", "Da", "Nu");
        if (!ok) return;

        try
        {
            IsBusy = true;
            await _service.DeleteAsync(dto.Id);
            await LoadAsync();
        }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsBusy = false; }
    }
}
