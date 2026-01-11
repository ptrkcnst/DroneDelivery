using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using DroneDelivery.Mobile.Models;
using DroneDelivery.Mobile.Services;

namespace DroneDelivery.Mobile.ViewModels;

public partial class OrdersViewModel : BaseViewModel
{
    private readonly IOrderService _service;

    public ObservableCollection<OrderDto> Items { get; } = new();

    public OrdersViewModel(IOrderService service)
    {
        _service = service;
        Title = "Orders";
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
            foreach (var o in list) Items.Add(o);
        }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task AddAsync()
        => await Shell.Current.GoToAsync(nameof(Pages.OrderEditPage));

    [RelayCommand]
    private async Task EditAsync(OrderDto dto)
        => await Shell.Current.GoToAsync($"{nameof(Pages.OrderEditPage)}?id={dto.Id}");

    [RelayCommand]
    private async Task DeleteAsync(OrderDto dto)
    {
        var ok = await Shell.Current.DisplayAlert("Confirm", $"Ștergi comanda #{dto.Id}?", "Da", "Nu");
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
