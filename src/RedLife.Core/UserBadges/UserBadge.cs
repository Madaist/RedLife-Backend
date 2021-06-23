using Abp.Domain.Entities;
using RedLife.Authorization.Users;
using RedLife.Core.Badges;

namespace RedLife.Core.UserBadges
{
    public class UserBadge : Entity
    {
        public long UserId { get; set; }
        public int BadgeId { get; set; }

        public virtual User User { get; set; }
        public virtual Badge Badge { get; set; }
    }
}
