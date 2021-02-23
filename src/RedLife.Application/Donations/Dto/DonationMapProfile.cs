using AutoMapper;
using RedLife.Core.Donations;

namespace RedLife.Application.Donations.Dto
{
    public class DonationMapProfile : Profile
    {
        public DonationMapProfile()
        {
            CreateMap<Donation, DonationDto>()
                .ForMember(u => u.CenterName, options => options.MapFrom(input => input.Center.InstitutionName))
                .ForMember(u => u.DonorFirstName, options => options.MapFrom(input => input.Donor.Name))
                .ForMember(u => u.DonorLastName, options => options.MapFrom(input => input.Donor.Surname))
                .ForMember(u => u.Date, options => options.MapFrom(input => input.Date.ToLocalTime().ToString("yyyy-MM-dd")));
        }
    }
        
}
