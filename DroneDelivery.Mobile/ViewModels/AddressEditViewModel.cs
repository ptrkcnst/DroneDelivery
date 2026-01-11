using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DroneDelivery.Mobile.Models;
using DroneDelivery.Mobile.Services;

namespace DroneDelivery.Mobile.ViewModels;

[QueryProperty(nameof(Id), "id")]
public partial class AddressEditViewModel : BaseViewModel
{
    private readonly IAddressService _service;

    [ObservableProperty] private int id;
    [ObservableProperty] private string city = "";
    [ObservableProperty] private string street = "";
    [ObservableProperty] private string zip = "";
    [ObservableProperty] private string notes = "";

    public AddressEditViewModel(IAddressService service)
    {
        _service = service;
        Title = "Edit Address";
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        Error = "";

        // VALIDARE
        if (string.IsNullOrWhiteSpace(City) || string.IsNullOrWhiteSpace(Street))
        {
            Error = "City și Street sunt obligatorii.";
            return;
        }

        try
        {
            IsBusy = true;

            var dto = new AddressDto
            {
                Id = Id,
                City = City.Trim(),
                Street = Street.Trim(),
                Zip = Zip.Trim(),
                Notes = Notes.Trim()
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
