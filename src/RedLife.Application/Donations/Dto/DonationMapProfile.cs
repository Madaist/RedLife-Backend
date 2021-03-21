using AutoMapper;
using RedLife.Core.Donations;
using RedLife.Core.PdfHelper;
using System.Globalization;

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
                .ForMember(u => u.Date, options => options.MapFrom(input => input.Date.ToLocalTime().ToString("yyyy-MM-dd")))
                .ForMember(u => u.Type, options => options.MapFrom(input => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.Type.ToLower()).Replace("_", " ")))
                .ForMember(u => u.MedicalTestsResult, options => options.MapFrom(input => PDFUtils.GetPdfOrNull(input.Id)));
        }

        
    }
        
}
