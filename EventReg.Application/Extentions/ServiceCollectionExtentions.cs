using EventReg.Application.Mapping;
using EventReg.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace EventReg.Application.Extentions;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var asssembly = Assembly.GetExecutingAssembly();

        var serviceTypes = asssembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Service"))
            .ToList();

        foreach (var serviceType in serviceTypes)
        {
            var iface = serviceType.GetInterfaces().FirstOrDefault(e => e.Name == $"I{serviceType.Name}");

            if (iface != null)
            {
                services.AddScoped(iface, serviceType);
            }
        }

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        services.AddValidatorsFromAssembly(asssembly);

        return services;
    }
}
