namespace EventReg.Application.DTOs;

public class RegistrationDto
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int UserId { get; set; }
    public string AttendeeName { get; set; }
    public string AttendeeEmail { get; set; }
}

public class RegistrationCreateDto
{
    public int EventId { get; set; }
    public int? UserId { get; set; }
    public string AttendeeName { get; set; }
    public string AttendeeEmail { get; set; }
    public string QRCode { get; set; }
    public DateTime RegisteredAt { get; set; }
}

public class RegistrationUpdateDto
{
    public string AttendeeName { get; set; }
    public string AttendeeEmail { get; set; }
}