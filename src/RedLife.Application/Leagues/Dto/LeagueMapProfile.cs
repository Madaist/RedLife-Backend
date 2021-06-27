using AutoMapper;
using RedLife.Core.Leagues;

namespace RedLife.Application.Leagues.Dto
{
    public class LeagueMapProfile : Profile
    {
        public LeagueMapProfile()
        {
            CreateMap<League, LeagueDto>();
            CreateMap<LeagueDto, League>();
        }
    }
}
