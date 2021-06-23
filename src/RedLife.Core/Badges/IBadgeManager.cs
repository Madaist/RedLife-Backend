using RedLife.Authorization.Users;
using RedLife.Core.Donations;
using System.Collections.Generic;
using System.Linq;

namespace RedLife.Core.Badges
{
    public interface IBadgeManager
    {
        public void AssignBadges(User user);
        public bool Check3DonationsIn9MonthsBadge(User user, ICollection<string> assignedBadges, IOrderedQueryable<Donation> userDonations);
        public bool CheckFirstSpecialDonationBadge(User user, ICollection<string> assignedBadges, IOrderedQueryable<Donation> userDonations);
        public bool CheckCovidPlasmaDonationBadge(User user, ICollection<string> assignedBadges, IOrderedQueryable<Donation> userDonations);
        public bool CheckHolidayDonationBadge(User user, ICollection<string> assignedBadges, IOrderedQueryable<Donation> userDonations);
        public bool CheckDonationAfter3MonthsBadge(User user, ICollection<string> assignedBadges, IOrderedQueryable<Donation> userDonations);
        public bool CheckDonationAfterLongTimeBadge(User user, ICollection<string> assignedBadges, IOrderedQueryable<Donation> userDonations);
        public bool CheckFirstDonationBadge(User user, ICollection<string> assignedBadges, IOrderedQueryable<Donation> userDonations);
        public void AssignBadgeToUser(string badgeName, User user);

        public ICollection<string> GetAssignedBadges(User user);
        public ICollection<Badge> GetAll();
    }
}
