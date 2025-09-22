using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using EventReg.Persistence;
using Microsoft.Extensions.Configuration;

namespace EventReg.Persistence.Extentions;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

        return services;
    }
}
