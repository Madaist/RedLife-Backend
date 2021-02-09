using AutoMapper;
using RedLife.Core.Appointments;
using System;

namespace RedLife.Application.Appointments.Dto
{
    public class AppointmentMapProfile : Profile
    {
        public AppointmentMapProfile()
        {
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(u => u.Id, options => options.MapFrom(input => input.Id))
                .ForMember(u => u.CenterName, options => options.MapFrom(input => input.Center.InstitutionName))
                .ForMember(u => u.CenterId, options => options.MapFrom(input => input.CenterId))
                .ForMember(u => u.DonorId, options => options.MapFrom(input => input.DonorId))
                .ForMember(u => u.DonorName, options => options.MapFrom(input => input.Donor.UserName))
                .ForMember(u => u.Date, options => options.MapFrom(input => input.Date.ToString("dd/M/yyyy")));

            CreateMap<AppointmentDto, CreateAppointmentDto>()
               .ForMember(u => u.DonorId, options => options.MapFrom(input => input.DonorId))
               .ForMember(u => u.CenterId, options => options.MapFrom(input => input.CenterId))
               .ForMember(u => u.Date, options => options.MapFrom(input => Convert.ToDateTime(input.Date)));

            CreateMap<Appointment, CreateAppointmentDto>()
              .ForMember(u => u.DonorId, options => options.MapFrom(input => input.DonorId))
              .ForMember(u => u.CenterId, options => options.MapFrom(input => input.CenterId))
              .ForMember(u => u.Date, options => options.MapFrom(input => input.Date));

            CreateMap<CreateAppointmentDto, Appointment>()
              .ForMember(u => u.DonorId, options => options.MapFrom(input => input.DonorId))
              .ForMember(u => u.CenterId, options => options.MapFrom(input => input.CenterId))
              .ForMember(u => u.Date, options => options.MapFrom(input => input.Date));

            CreateMap<UpdateAppointmentDto, Appointment>()
             .ForMember(u => u.DonorId, options => options.MapFrom(input => input.DonorId))
             .ForMember(u => u.CenterId, options => options.MapFrom(input => input.CenterId))
             .ForMember(u => u.Date, options => options.MapFrom(input => input.Date));
        }
    }
}
