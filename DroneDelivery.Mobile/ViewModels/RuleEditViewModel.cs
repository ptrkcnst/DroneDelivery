using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DroneDelivery.Mobile.Models;
using DroneDelivery.Mobile.Services;

namespace DroneDelivery.Mobile.ViewModels;

[QueryProperty(nameof(Id), "id")]
public partial class RuleEditViewModel : BaseViewModel
{
    private readonly IRuleService _service;

    [ObservableProperty] private int id;
    [ObservableProperty] private int daysBefore = 1;
    [ObservableProperty] private bool enabled = true;

    public RuleEditViewModel(IRuleService service)
    {
        _service = service;
        Title = "Edit Rule";
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        Error = "";

        // VALIDARE: doar 1/3/5/7
        var allowed = new[] { 1, 3, 5, 7 };
        if (!allowed.Contains(DaysBefore))
        {
            Error = "DaysBefore trebuie să fie 1, 3, 5 sau 7.";
            return;
        }

        try
        {
            IsBusy = true;
            var dto = new NotificationRuleDto
            {
                Id = Id,
                DaysBefore = DaysBefore,
                Enabled = Enabled
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
