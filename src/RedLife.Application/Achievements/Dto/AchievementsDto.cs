using RedLife.Application.Badges.Dto;
using RedLife.Application.Leagues.Dto;
using RedLife.Users.Dto;
using System.Collections.Generic;

namespace RedLife.Application.Achievements.Dto
{
    public class AchievementsDto
    {
        public int Points { get; set; }
        public int PeopleHelpedCount { get; set; }
        public LeagueDto League { get; set; }
        public LeagueDto NextLeague { get; set; }
        public ICollection<BadgeDto> AssignedBadges { get; set; }
        public ICollection<BadgeDto> UnassignedBadges { get; set; }
        public ICollection<UserDto> TopUsers { get; set; }

    }
}
