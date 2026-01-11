using System.Collections.Concurrent;
using DroneDelivery.Mobile.Helpers;
using DroneDelivery.Mobile.Models;

namespace DroneDelivery.Mobile.Services;

/// <summary>
/// Backend fake in-memory. Scop: aplicatia sa mearga complet (navigare + CRUD) fara API real.
/// </summary>
public class MockApiClient : IApiClient
{
    private static int _addressId = 3;
    private static int _orderId = 2;
    private static int _ruleId = 3;

    private static readonly List<AddressDto> Addresses = new()
    {
        new AddressDto { Id = 1, City = "Bucharest", Street = "Bulevardul Unirii 10", Zip = "030100", Notes = "HQ" },
        new AddressDto { Id = 2, City = "Cluj-Napoca", Street = "Str. Memorandumului 5", Zip = "400114", Notes = "Client" },
        new AddressDto { Id = 3, City = "Iasi", Street = "Str. Stefan cel Mare 1", Zip = "700259", Notes = "Warehouse" },
    };

    private static readonly List<OrderDto> Orders = new()
    {
        new OrderDto { Id = 1, AddressId = 1, CreatedAt = DateTime.UtcNow.AddDays(-1), TotalWeightKg = 2.5, Status = "Scheduled", ScheduledAt = DateTime.UtcNow.AddHours(4) },
        new OrderDto { Id = 2, AddressId = 2, CreatedAt = DateTime.UtcNow.AddDays(-3), TotalWeightKg = 1.2, Status = "Delivered" },
    };

    private static readonly List<NotificationRuleDto> Rules = new()
    {
        new NotificationRuleDto { Id = 1, DaysBefore = 1, Enabled = true },
        new NotificationRuleDto { Id = 2, DaysBefore = 3, Enabled = true },
        new NotificationRuleDto { Id = 3, DaysBefore = 7, Enabled = false },
    };

    private static readonly List<NotificationDto> Notifications = new()
    {
        new NotificationDto { Id = 1, DeliveryId = 101, SendAt = DateTime.UtcNow.AddHours(2), Status = "Pending", Type = "Reminder" },
        new NotificationDto { Id = 2, DeliveryId = 102, SendAt = DateTime.UtcNow.AddHours(-6), SentAt = DateTime.UtcNow.AddHours(-6), Status = "Sent", Type = "Reminder" },
    };

    public Task SetBearerTokenAsync() => Task.CompletedTask;

    public Task<T> GetAsync<T>(string path)
    {
        object? result = path switch
        {
            ApiEndpoints.Addresses => Addresses.ToList(),
            ApiEndpoints.Orders => Orders.ToList(),
            ApiEndpoints.NotificationRules => Rules.ToList(),
            ApiEndpoints.Notifications => Notifications.ToList(),
            _ => null
        };

        if (result is null)
            throw new InvalidOperationException($"[MOCK] Unknown GET path: {path}");

        return Task.FromResult((T)result);
    }

    public Task<T> PostAsync<T>(string path, object body)
    {
        if (path == ApiEndpoints.AuthLogin)
        {
            var req = body as LoginRequest;
            if (req is null || string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
                throw new InvalidOperationException("Email si parola sunt obligatorii.");

            object resp = new AuthResponse
            {
                Token = "dev-token",
                Role = "User",
                UserId = 1,
                Email = req.Email
            };
            return Task.FromResult((T)resp);
        }

        if (path == ApiEndpoints.Addresses)
        {
            var dto = Ensure<AddressDto>(body);
            dto.Id = Interlocked.Increment(ref _addressId);
            Addresses.Add(dto);
            return Task.FromResult((T)(object)dto);
        }

        if (path == ApiEndpoints.Orders)
        {
            var dto = Ensure<OrderDto>(body);
            dto.Id = Interlocked.Increment(ref _orderId);
            if (dto.CreatedAt == default) dto.CreatedAt = DateTime.UtcNow;
            Orders.Add(dto);
            return Task.FromResult((T)(object)dto);
        }

        if (path == ApiEndpoints.NotificationRules)
        {
            var dto = Ensure<NotificationRuleDto>(body);
            dto.Id = Interlocked.Increment(ref _ruleId);
            Rules.Add(dto);
            return Task.FromResult((T)(object)dto);
        }

        throw new InvalidOperationException($"[MOCK] Unknown POST path: {path}");
    }

    public Task PutAsync(string path, object body)
    {
        if (path.StartsWith(ApiEndpoints.Addresses + "/"))
        {
            var dto = Ensure<AddressDto>(body);
            var idx = Addresses.FindIndex(a => a.Id == dto.Id);
            if (idx < 0) throw new InvalidOperationException($"Address #{dto.Id} not found");
            Addresses[idx] = dto;
            return Task.CompletedTask;
        }

        if (path.StartsWith(ApiEndpoints.Orders + "/"))
        {
            var dto = Ensure<OrderDto>(body);
            var idx = Orders.FindIndex(o => o.Id == dto.Id);
            if (idx < 0) throw new InvalidOperationException($"Order #{dto.Id} not found");
            Orders[idx] = dto;
            return Task.CompletedTask;
        }

        if (path.StartsWith(ApiEndpoints.NotificationRules + "/"))
        {
            var dto = Ensure<NotificationRuleDto>(body);
            var idx = Rules.FindIndex(r => r.Id == dto.Id);
            if (idx < 0) throw new InvalidOperationException($"Rule #{dto.Id} not found");
            Rules[idx] = dto;
            return Task.CompletedTask;
        }

        throw new InvalidOperationException($"[MOCK] Unknown PUT path: {path}");
    }

    public Task DeleteAsync(string path)
    {
        if (path.StartsWith(ApiEndpoints.Addresses + "/") && int.TryParse(path.Split('/').Last(), out var aId))
        {
            Addresses.RemoveAll(a => a.Id == aId);
            return Task.CompletedTask;
        }

        if (path.StartsWith(ApiEndpoints.Orders + "/") && int.TryParse(path.Split('/').Last(), out var oId))
        {
            Orders.RemoveAll(o => o.Id == oId);
            return Task.CompletedTask;
        }

        if (path.StartsWith(ApiEndpoints.NotificationRules + "/") && int.TryParse(path.Split('/').Last(), out var rId))
        {
            Rules.RemoveAll(r => r.Id == rId);
            return Task.CompletedTask;
        }

        if (path.StartsWith(ApiEndpoints.Notifications + "/") && int.TryParse(path.Split('/').Last(), out var nId))
        {
            Notifications.RemoveAll(n => n.Id == nId);
            return Task.CompletedTask;
        }

        throw new InvalidOperationException($"[MOCK] Unknown DELETE path: {path}");
    }

    private static TDto Ensure<TDto>(object body) where TDto : class
        => body as TDto ?? throw new InvalidOperationException($"Invalid body type. Expected {typeof(TDto).Name}");
}
