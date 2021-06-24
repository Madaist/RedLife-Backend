using RedLife.Core.UserBadges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedLife.EntityFrameworkCore.Seed.Tenants
{
    public class UserBadgesCreator
    {
        private readonly RedLifeDbContext _context;

        public UserBadgesCreator(RedLifeDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateUserBadges();
        }

        private void CreateUserBadges()
        {
            if (_context.UserBadges.Count() < 3)
            {
                var userBadge1 = new UserBadge
                {
                    UserId = 2990407460021,
                    BadgeId = 7
                };
                var userBadge2 = new UserBadge
                {
                    UserId = 2990407460021,
                    BadgeId = 2
                };
                var userBadge3 = new UserBadge
                {
                    UserId = 1990305329641,
                    BadgeId = 7
                };
                var userBadge4 = new UserBadge
                {
                    UserId = 2991230465121,
                    BadgeId = 7
                };
                var userBadge5 = new UserBadge
                {
                    UserId = 2991230465121,
                    BadgeId = 2
                };
                var userBadge6 = new UserBadge
                {
                    UserId = 2991230465121,
                    BadgeId = 5
                };
                var userBadge7 = new UserBadge
                {
                    UserId = 1991207165828,
                    BadgeId = 2
                };
                var userBadge8 = new UserBadge
                {
                    UserId = 1991207165828,
                    BadgeId = 7
                };
                var userBadge9 = new UserBadge
                {
                    UserId = 1991207165828,
                    BadgeId = 3
                };
                var userBadge10 = new UserBadge
                {
                    UserId = 2961202115227,
                    BadgeId = 7
                };

                _context.UserBadges.AddRange(new UserBadge[] {userBadge1, userBadge2, userBadge3,
                    userBadge4, userBadge5, userBadge6, userBadge7, userBadge8, userBadge9, userBadge10});
                _context.SaveChanges();
            }
        }
    }
}
