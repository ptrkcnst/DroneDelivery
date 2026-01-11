using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DroneDelivery.Mobile.Models;
using DroneDelivery.Mobile.Services;

namespace DroneDelivery.Mobile.ViewModels;

[QueryProperty(nameof(Id), "id")]
public partial class OrderEditViewModel : BaseViewModel
{
    private readonly IOrderService _service;

    [ObservableProperty] private int id;
    [ObservableProperty] private int addressId;
    [ObservableProperty] private double totalWeightKg;
    [ObservableProperty] private string status = "Draft";

    public OrderEditViewModel(IOrderService service)
    {
        _service = service;
        Title = "Edit Order";
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        Error = "";

        // VALIDARE
        if (AddressId <= 0)
        {
            Error = "Trebuie să selectezi AddressId (>=1).";
            return;
        }
        if (TotalWeightKg <= 0)
        {
            Error = "Greutatea trebuie să fie > 0.";
            return;
        }

        try
        {
            IsBusy = true;

            var dto = new OrderDto
            {
                Id = Id,
                AddressId = AddressId,
                TotalWeightKg = TotalWeightKg,
                Status = string.IsNullOrWhiteSpace(Status) ? "Draft" : Status.Trim()
            };

            if (Id == 0)
                await _service.CreateAsync(dto);
            else
                await _service.UpdateAsync(dto);

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsBusy = false; }
    }
}
