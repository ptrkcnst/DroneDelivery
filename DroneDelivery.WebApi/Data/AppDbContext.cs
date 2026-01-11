using DroneDelivery.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DroneDelivery.WebApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<NotificationRule> NotificationRules => Set<NotificationRule>();
    public DbSet<Notification> Notifications => Set<Notification>();
}
