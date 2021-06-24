using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Abp.Runtime.Session;
using RedLife.Application.Achievements.Dto;
using RedLife.Application.Badges.Dto;
using RedLife.Application.Leagues.Dto;
using RedLife.Authorization;
using RedLife.Authorization.Users;
using RedLife.Core.Achievements;
using RedLife.Core.Leagues;
using RedLife.Users.Dto;
using System.Collections.Generic;
using System.Linq;
using static RedLife.Authorization.Roles.StaticRoleNames;

namespace RedLife.Application.Achievements
{
    [AbpAuthorize]
    public class AchievementsAppService : IAchievementsAppService
    {
        public IAbpSession AbpSession { get; set; }
        private readonly IAchievementsManager _achievementsManager;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<League> _leagueRepository;
        private readonly IObjectMapper _objectMapper;
        private readonly UserManager _userManager;

        public AchievementsAppService( 
            IAchievementsManager achievementsManager,
            IRepository<User, long> userRepository,
            IRepository<League> leagueRepository,
            IObjectMapper objectMapper,
            UserManager userManager) 
        {
            AbpSession = NullAbpSession.Instance;
            _achievementsManager = achievementsManager;
            _userRepository = userRepository;
            _leagueRepository = leagueRepository;
            _objectMapper = objectMapper;
            _userManager = userManager;
        }


        [AbpAuthorize(PermissionNames.Donor)]
        public AchievementsDto GetAchievements()
        {
            long donorId = AbpSession.UserId ?? 0;
            AchievementsDto achievements = ComputeAchievements(donorId);
            return achievements;
        }

        [AbpAuthorize(PermissionNames.Donor)]
        public ICollection<UserDto> GetTopByLeagueId(int leagueId)
        {
            return GetLeaderboardByLeagueId(leagueId);
        }

        private AchievementsDto ComputeAchievements(long donorId)
        {
            return new AchievementsDto
            {
                Points = _userRepository.Get(donorId).Points,
                PeopleHelpedCount = _achievementsManager.GetPeopleHelped(donorId),
                League = GetUserLeague(donorId),
                NextLeague = GetNextLeague(donorId),
                AssignedBadges = GetAssignedBadges(donorId),
                UnassignedBadges = GetUnassignedBadges(donorId),
                TopUsers = GetLeaderboard(donorId)
            };
        }

        private LeagueDto GetUserLeague(long donorId)
        {
            var league = _userRepository.Get(donorId).League;
            return _objectMapper.Map<LeagueDto>(league);
        }

        private LeagueDto GetNextLeague(long donorId)
        {
            var maxPointsCurrentLeague = _userRepository.Get(donorId).League.MaxPoints;
            var nextLeague = _leagueRepository.FirstOrDefault(l => l.MinPoints == maxPointsCurrentLeague + 1);
            return _objectMapper.Map<LeagueDto>(nextLeague);
        }

        private ICollection<BadgeDto> GetUnassignedBadges(long donorId)
        {
            var badges = _achievementsManager.GetUnassignedBadges(donorId);
            return _objectMapper.Map<ICollection<BadgeDto>>(badges); ;
        }

        private ICollection<BadgeDto> GetAssignedBadges(long donorId)
        {
            var badges = _achievementsManager.GetAssignedBadges(donorId);
            return _objectMapper.Map<ICollection<BadgeDto>>(badges);
        }

        private ICollection<UserDto> GetLeaderboard(long donorId)
        {
            League league = _userRepository.Get(donorId).League;
            return GetLeaderboardByLeagueId(league.Id);
        }

        private ICollection<UserDto> GetLeaderboardByLeagueId(int leagueId)
        {
            var donors = _userManager.GetUsersInRoleAsync(Tenants.Donor).Result;
            var topUsers = donors.Where(x => x.LeagueId == leagueId).OrderByDescending(x => x.Points).ToList();

            return _objectMapper.Map<List<UserDto>>(topUsers);
        }
    }
}
