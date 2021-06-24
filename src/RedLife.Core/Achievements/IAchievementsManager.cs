using RedLife.Authorization.Users;
using RedLife.Core.Badges;
using System.Collections.Generic;

namespace RedLife.Core.Achievements
{
    public interface IAchievementsManager
    {
        public int GetPeopleHelped(long donorId);
        public ICollection<Badge> GetUnassignedBadges(long donorId);
        public ICollection<Badge> GetAssignedBadges(long donorId);
        public void UpdateLeagueandBadges(User user);
        public void UpdateLeague(User user);
    }
}
