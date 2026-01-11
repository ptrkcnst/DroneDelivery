using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DroneDelivery.Mobile.Models;
using DroneDelivery.Mobile.Services;
using Microsoft.Maui.Storage;

namespace DroneDelivery.Mobile.ViewModels;

public partial class DeliveryRequestViewModel : BaseViewModel
{
    private const string LastLoginEmailKey = "LastLoginEmail";

    private readonly IAddressService _addressService;
    private readonly IOrderService _orderService;

    [ObservableProperty] private DateTime deliveryDate = DateTime.Today.AddDays(1);
    [ObservableProperty] private string email = "";
    [ObservableProperty] private string label = "";
    [ObservableProperty] private string street = "";
    [ObservableProperty] private string city = "";
    [ObservableProperty] private string country = "Romania";
    [ObservableProperty] private string zip = "";
    [ObservableProperty] private string notes = "";
    [ObservableProperty] private string totalWeightKg = "1.0";

    public DeliveryRequestViewModel(IAddressService addressService, IOrderService orderService)
    {
        _addressService = addressService;
        _orderService = orderService;
        Title = "Schedule Delivery";

        Email = Preferences.Get(LastLoginEmailKey, string.Empty);
    }

    [RelayCommand]
    private async Task SubmitAsync()
    {
        Error = "";

        if (string.IsNullOrWhiteSpace(Street) || string.IsNullOrWhiteSpace(City))
        {
            Error = "Street and city are required.";
            return;
        }
        if (string.IsNullOrWhiteSpace(Email))
        {
            Error = "Email is required.";
            return;
        }

        if (!double.TryParse(TotalWeightKg, out var weight) || weight <= 0)
        {
            Error = "Weight must be a positive number.";
            return;
        }

        try
        {
            IsBusy = true;

            var address = await _addressService.CreateAsync(new AddressDto
            {
                Label = string.IsNullOrWhiteSpace(Label) ? "Delivery address" : Label.Trim(),
                Street = Street.Trim(),
                City = City.Trim(),
                Country = string.IsNullOrWhiteSpace(Country) ? "Romania" : Country.Trim(),
                Zip = Zip.Trim(),
                Notes = Notes.Trim()
            });

            await _orderService.CreateAsync(new OrderDto
            {
                AddressId = address.Id,
                Email = Email.Trim(),
                TotalWeightKg = weight,
                Status = "Scheduled",
                CreatedAt = DateTime.UtcNow,
                ScheduledAt = DeliveryDate.Date
            });

            await Shell.Current.DisplayAlert("Success", "Delivery scheduled.", "OK");

            Street = "";
            City = "";
            Zip = "";
            Notes = "";
            Label = "";
            TotalWeightKg = "1.0";
            DeliveryDate = DateTime.Today.AddDays(1);
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
