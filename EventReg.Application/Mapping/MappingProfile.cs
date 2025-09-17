using AutoMapper;
using EventReg.Domain.Entities;
using EventReg.Application.DTOs;

namespace EventReg.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Registration, RegistrationDto>().ReverseMap();
        CreateMap<RegistrationCreateDto, Registration>();
        CreateMap<RegistrationUpdateDto, Registration>();

        CreateMap<Event, EventDto>().ReverseMap();
        CreateMap<EventCreateDto, Event>();
        CreateMap<EventUpdateDto, Event>();
    }
}
