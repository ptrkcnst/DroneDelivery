namespace DroneDelivery.Mobile.Models;

// ---------- AUTH ----------
public class LoginRequest
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}

public class AuthResponse
{
    public string Token { get; set; } = "";
    public string Role { get; set; } = "";
    public int UserId { get; set; }
    public string Email { get; set; } = "";
}

// ---------- ADDRESSES ----------
public class AddressDto
{
    public int Id { get; set; }
    public string Label { get; set; } = "";
    public string City { get; set; } = "";
    public string Street { get; set; } = "";
    public string Country { get; set; } = "";
    public string Zip { get; set; } = "";
    public double? Lat { get; set; }
    public double? Lng { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string Notes { get; set; } = "";
}

// ---------- ORDERS ----------
public class OrderDto
{
    public int Id { get; set; }
    public int AddressId { get; set; }
    public string Email { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public double TotalWeightKg { get; set; }
    public string Status { get; set; } = "Draft"; // Draft/Scheduled/InDelivery/Delivered/Canceled

    // optional: dacă backend-ul trimite detalii
    public DateTime? ScheduledAt { get; set; }
}

// ---------- NOTIFICATION RULES ----------
public class NotificationRuleDto
{
    public int Id { get; set; }
    public int DaysBefore { get; set; } // 1,3,5,7
    public bool Enabled { get; set; }
}

// ---------- NOTIFICATIONS ----------
public class NotificationDto
{
    public int Id { get; set; }
    public int DeliveryId { get; set; }
    public DateTime SendAt { get; set; }
    public DateTime? SentAt { get; set; }
    public string Status { get; set; } = "Pending"; // Pending/Sent/Failed
    public string Type { get; set; } = "Reminder";
}
