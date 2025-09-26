namespace EventReg.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Role { get; set; } = "Attendee"; //Admin, Attendee
    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
