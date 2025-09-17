namespace EventReg.Domain.Entities;

public class Event
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Location { get; set; } = null!;
    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
