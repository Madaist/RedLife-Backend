using AutoMapper;
using RedLife.Core.Badges;

namespace RedLife.Application.Badges.Dto
{
    public class BadgeMapProfile : Profile
    {
        public BadgeMapProfile()
        {
            CreateMap<Badge, BadgeDto>();
        }
    }
}
