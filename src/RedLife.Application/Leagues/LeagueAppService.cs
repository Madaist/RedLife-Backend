using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using RedLife.Application.Leagues.Dto;
using RedLife.Authorization;
using RedLife.Core.Leagues;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedLife.Application.Leagues
{
    [AbpAuthorize]
    public class LeagueAppService : ILeagueAppService
    {
        private readonly IRepository<League> _leagueRepository;
        private readonly IObjectMapper _objectMapper;


        public LeagueAppService(
            IRepository<League> leagueRepository,
            IObjectMapper objectMapper)
        {
            _leagueRepository = leagueRepository;
            _objectMapper = objectMapper;
        }

        [AbpAuthorize(PermissionNames.Donor)]
        public ICollection<LeagueDto> GetAll()
        {
            var leagues = _leagueRepository.GetAll();
            return _objectMapper.Map<ICollection<LeagueDto>>(leagues);
        }



    }
}
