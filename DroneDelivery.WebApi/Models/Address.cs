namespace DroneDelivery.WebApi.Models;

public class Address
{
    public int Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}
