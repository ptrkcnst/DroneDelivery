using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using DroneDelivery.Mobile.Models;
using DroneDelivery.Mobile.Services;

namespace DroneDelivery.Mobile.ViewModels;

public partial class AddressesViewModel : BaseViewModel
{
    private readonly IAddressService _service;

    public ObservableCollection<AddressDto> Items { get; } = new();

    public AddressesViewModel(IAddressService service)
    {
        _service = service;
        Title = "Addresses";
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
            foreach (var a in list) Items.Add(a);
        }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task AddAsync()
        => await Shell.Current.GoToAsync(nameof(Pages.AddressEditPage));

    [RelayCommand]
    private async Task EditAsync(AddressDto dto)
        => await Shell.Current.GoToAsync($"{nameof(Pages.AddressEditPage)}?id={dto.Id}");

    [RelayCommand]
    private async Task DeleteAsync(AddressDto dto)
    {
        var ok = await Shell.Current.DisplayAlert("Confirm", $"Ștergi adresa #{dto.Id}?", "Da", "Nu");
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
