using Abp.Domain.Entities;
using RedLife.Core.UserBadges;
using System.Collections.Generic;

namespace RedLife.Core.Badges
{
    public class Badge : Entity
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Points { get; set; }

        public virtual ICollection<UserBadge> UserBadges { get; set; }
    }
}
