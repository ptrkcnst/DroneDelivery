namespace DroneDelivery.WebApi.Models;

public class Order
{
    public int Id { get; set; }
    public int AddressId { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public double TotalWeightKg { get; set; }
    public string Status { get; set; } = "Draft";
    public DateTime? ScheduledAt { get; set; }
}
