using EventReg.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventReg.Persistence
{
    public class DataSeed
    {
        public static async Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (!await dbContext.Events.AnyAsync())
            {
                var sampleEvent = new Event
                {
                    Title = "Tech Conference 2025",
                    Description = "Annual technology conference with workshops and networking.",
                    StartDate = new DateTime(2025, 11, 17),
                    EndDate = new DateTime(2025, 11, 17),
                    Location = "Online"
                };

                dbContext.Events.Add(sampleEvent);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
