using AutoMapper;
using RedLife.Core.Appointments;

namespace RedLife.Application.Appointments.Dto
{
    public class AppointmentMapProfile : Profile
    {
        public AppointmentMapProfile()
        {
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(u => u.CenterName, options => options.MapFrom(input => input.Center.InstitutionName))
                .ForMember(u => u.DonorLastName, options => options.MapFrom(input => input.Donor.Surname))
                .ForMember(u => u.DonorFirstName, options => options.MapFrom(input => input.Donor.Name))
                .ForMember(u => u.CenterAddress, options => options.MapFrom(input => 
                           input.Center.City + ", " + input.Center.Street + " " +  input.Center.Number))
                .ForMember(u => u.Date, options => options.MapFrom(input => input.Date.ToLocalTime().ToString("yyyy-MM-dd")));
        }
    }
}
