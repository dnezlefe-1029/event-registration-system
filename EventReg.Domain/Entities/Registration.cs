namespace EventReg.Domain.Entities;

public class Registration
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int? UserId { get; set; }
    public string AttendeeName { get; set; } = null!;
    public string AttendeeEmail { get; set; } = null!;
    public string QRCode { get; set; } = null!;
    public DateTime RegisteredAt { get; set; }
}
