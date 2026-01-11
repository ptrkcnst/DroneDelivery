using DroneDelivery.WebApi.Models;

namespace DroneDelivery.WebApi.Dtos;

public record OrderAdminDto(
    int Id,
    int AddressId,
    string Email,
    DateTime CreatedAt,
    double TotalWeightKg,
    string Status,
    DateTime? ScheduledAt,
    Address? Address
);
