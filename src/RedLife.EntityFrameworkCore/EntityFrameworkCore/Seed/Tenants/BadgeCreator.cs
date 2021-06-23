using RedLife.Core.Badges;
using System.Linq;

namespace RedLife.EntityFrameworkCore.Seed.Tenants
{
    public class BadgeCreator
    {
        private readonly RedLifeDbContext _context;

        public BadgeCreator(RedLifeDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateBadges();
        }

        private void CreateBadges()
        {
            if (_context.Badges.Count() < 3)
            {
                var badge1 = new Badge
                {
                    Name = BadgeTypes.FirstDonationBadge,
                    Points = 20,
                    Icon = "https://i.ibb.co/sR1DLrn/first-donation.png"
                };
                var badge2 = new Badge
                {
                    Name = BadgeTypes.DonationAfterLongTime,
                    Points = 10,
                    Icon = "https://i.ibb.co/n0g1vd6/donation-long-time.png"
                };
                var badge3 = new Badge
                {
                    Name = BadgeTypes.DonationAfter3Months,
                    Points = 40,
                    Icon = "https://i.ibb.co/CzCdhfC/three-months.png"
                };
                var badge4 = new Badge
                {
                    Name = BadgeTypes.HolidayDonation,
                    Points = 50,
                    Icon = "https://i.ibb.co/qWNvzRx/holiday-donation1.png"
                };
                var badge5 = new Badge
                {
                    Name = BadgeTypes.CovidPlasmaDonation,
                    Points = 30,
                    Icon = "https://i.ibb.co/QkPq2R9/covid-donation.png"
                };
                var badge6 = new Badge
                {
                    Name = BadgeTypes.FirstSpecialDonation,
                    Points = 35,
                    Icon = "https://i.ibb.co/5hrRy80/special-badge.png"
                };
                var badge7 = new Badge
                {
                    Name = BadgeTypes.ThreeDonationsIn9Months,
                    Points = 60,
                    Icon = "https://i.ibb.co/SJW0h2j/three-nine.png"
                };

                _context.Badges.AddRange(new Badge[] { badge1, badge2, badge3,
                    badge4, badge5, badge6, badge7});
                _context.SaveChanges();
            }
        }
    }
}
