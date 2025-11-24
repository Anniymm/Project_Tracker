using AutoMapper;
using Project3.Application.Common.DTOs;
using Project3.Domain.Entities;

namespace Project3.Application.Common.Mappings;

// yvela appointmenti rom davmapot Appointment -> AppointmentDto
// radgan statusi aris 
public class AppointmentProfile : Profile
{
    public AppointmentProfile()
    {
        CreateMap<Appointment, AppointmentDto>()
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToString()));
    }
}