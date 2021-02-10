using AutoMapper;
using RedLife.Core.Appointments;

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
                .ForMember(u => u.Date, options => options.MapFrom(input => input.Date.ToLocalTime().ToString("yyyy-MM-dd")));

        }
    }
}
