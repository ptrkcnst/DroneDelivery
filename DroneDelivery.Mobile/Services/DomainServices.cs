using DroneDelivery.Mobile.Helpers;
using DroneDelivery.Mobile.Models;

namespace DroneDelivery.Mobile.Services;

public interface IAddressService
{
    Task<List<AddressDto>> GetAllAsync();
    Task<AddressDto> CreateAsync(AddressDto dto);
    Task UpdateAsync(AddressDto dto);
    Task DeleteAsync(int id);
}

public class AddressService : IAddressService
{
    private readonly IApiClient _api;
    public AddressService(IApiClient api) => _api = api;

    public Task<List<AddressDto>> GetAllAsync()
        => _api.GetAsync<List<AddressDto>>(ApiEndpoints.Addresses);

    public Task<AddressDto> CreateAsync(AddressDto dto)
        => _api.PostAsync<AddressDto>(ApiEndpoints.Addresses, dto);

    public Task UpdateAsync(AddressDto dto)
        => _api.PutAsync(ApiEndpoints.AddressById(dto.Id), dto);

    public Task DeleteAsync(int id)
        => _api.DeleteAsync(ApiEndpoints.AddressById(id));
}

public interface IOrderService
{
    Task<List<OrderDto>> GetAllAsync();
    Task<OrderDto> CreateAsync(OrderDto dto);
    Task UpdateAsync(OrderDto dto);
    Task DeleteAsync(int id);
}

public class OrderService : IOrderService
{
    private readonly IApiClient _api;
    public OrderService(IApiClient api) => _api = api;

    public Task<List<OrderDto>> GetAllAsync()
        => _api.GetAsync<List<OrderDto>>(ApiEndpoints.Orders);

    public Task<OrderDto> CreateAsync(OrderDto dto)
        => _api.PostAsync<OrderDto>(ApiEndpoints.Orders, dto);

    public Task UpdateAsync(OrderDto dto)
        => _api.PutAsync(ApiEndpoints.OrderById(dto.Id), dto);

    public Task DeleteAsync(int id)
        => _api.DeleteAsync(ApiEndpoints.OrderById(id));
}

public interface IRuleService
{
    Task<List<NotificationRuleDto>> GetAllAsync();
    Task<NotificationRuleDto> CreateAsync(NotificationRuleDto dto);
    Task UpdateAsync(NotificationRuleDto dto);
    Task DeleteAsync(int id);
}

public class RuleService : IRuleService
{
    private readonly IApiClient _api;
    public RuleService(IApiClient api) => _api = api;

    public Task<List<NotificationRuleDto>> GetAllAsync()
        => _api.GetAsync<List<NotificationRuleDto>>(ApiEndpoints.NotificationRules);

    public Task<NotificationRuleDto> CreateAsync(NotificationRuleDto dto)
        => _api.PostAsync<NotificationRuleDto>(ApiEndpoints.NotificationRules, dto);

    public Task UpdateAsync(NotificationRuleDto dto)
        => _api.PutAsync(ApiEndpoints.RuleById(dto.Id), dto);

    public Task DeleteAsync(int id)
        => _api.DeleteAsync(ApiEndpoints.RuleById(id));
}

public interface INotificationService
{
    Task<List<NotificationDto>> GetAllAsync();
    Task DeleteAsync(int id);
}

public class NotificationService : INotificationService
{
    private readonly IApiClient _api;
    public NotificationService(IApiClient api) => _api = api;

    public Task<List<NotificationDto>> GetAllAsync()
        => _api.GetAsync<List<NotificationDto>>(ApiEndpoints.Notifications);

    public Task DeleteAsync(int id)
        => _api.DeleteAsync($"{ApiEndpoints.Notifications}/{id}");
}
