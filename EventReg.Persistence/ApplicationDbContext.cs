using Microsoft.EntityFrameworkCore;
using EventReg.Domain.Entities;

namespace EventReg.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Event> Events { get; set; } = null!;
    public DbSet<Registration> Registrations { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed Users
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "Admin Admin", Username = "admin", Email = "dnezle@gmail.com", PasswordHash = "hashedpassword", Role = "Admin" },
            new User { Id = 2, Name = "Test User", Username = "user", Email = "cecil94.doncillo@gmail.com", PasswordHash = "hashedpassword", Role = "Attendee" }
        );

        // Seed Events
        modelBuilder.Entity<Event>().HasData(
            new Event { Id = 1, Title = "Tech Conference 2025", Description = "Annual tech meetup", Location = "New York", StartDate = new DateTime(2025, 11, 15), EndDate = new DateTime(2025, 11, 17) },
            new Event { Id = 2, Title = "Music Festival", Description = "Outdoor live music event", Location = "Los Angeles", StartDate = new DateTime(2025, 12, 5), EndDate = new DateTime(2025, 12, 7) }
        );

        // Seed Registrations
        modelBuilder.Entity<Registration>().HasData(
            new Registration { Id = 1, EventId = 1, UserId = 2, AttendeeName = "John Doe", AttendeeEmail = "john@example.com", QRCode = "qrcode1.png", RegisteredAt = new DateTime(2025, 09, 16, 12, 0, 0, DateTimeKind.Utc) }
        );
    }
}
