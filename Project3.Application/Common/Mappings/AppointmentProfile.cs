// using AutoMapper;
// using Project3.Application.Common.DTOs;
// using Project3.Domain.Entities;
//
// namespace Project3.Application.Common.Mappings;
//
// // automapperebi profilebis mashin mwhirdeba roca entityshi da dtoshi propertebs sxvadasxva saxelebs varqmev
// // yvela appointmenti rom davmapot Appointment -> AppointmentDto
//
// public class AppointmentProfile : Profile
// {
//     public AppointmentProfile()
//     {
//         CreateMap<Appointment, GetAppointmentDto>()
//             .ForMember(dest => dest.Status,
//                 opt => opt.MapFrom(
//                     src => src.Status.ToString()));
//     }
// }