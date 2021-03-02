using AutoMapper;
using RedLife.Core.Transfusions;

namespace RedLife.Application.Transfusions.Dto
{
    public class TransfusionMapProfile : Profile
    {
        public TransfusionMapProfile()
        {
            CreateMap<Transfusion, TransfusionDto>()
                 .ForMember(u => u.Date, options => options.MapFrom(input => input.Date.ToLocalTime().ToString("yyyy-MM-dd")))
                 .ForMember(u => u.HospitalName, options => options.MapFrom(input => input.Hospital.InstitutionName));
        }
    }
}
