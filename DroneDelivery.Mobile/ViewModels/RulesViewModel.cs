using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using DroneDelivery.Mobile.Models;
using DroneDelivery.Mobile.Services;

namespace DroneDelivery.Mobile.ViewModels;

public partial class RulesViewModel : BaseViewModel
{
    private readonly IRuleService _service;
    public ObservableCollection<NotificationRuleDto> Items { get; } = new();

    public RulesViewModel(IRuleService service)
    {
        _service = service;
        Title = "Notification Rules";
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
            foreach (var r in list) Items.Add(r);
        }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task AddAsync()
        => await Shell.Current.GoToAsync(nameof(Pages.RuleEditPage));

    [RelayCommand]
    private async Task EditAsync(NotificationRuleDto dto)
        => await Shell.Current.GoToAsync($"{nameof(Pages.RuleEditPage)}?id={dto.Id}");

    [RelayCommand]
    private async Task DeleteAsync(NotificationRuleDto dto)
    {
        var ok = await Shell.Current.DisplayAlert("Confirm", $"Ștergi regula #{dto.Id}?", "Da", "Nu");
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
