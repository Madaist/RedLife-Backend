using Abp.Domain.Entities;
using RedLife.Authorization.Users;
using System.Collections.Generic;

namespace RedLife.Core.Leagues
{
    public class League : Entity
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public int MinPoints { get; set; }
        public int MaxPoints { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
