using Abp.Dependency;
using Abp.Domain.Repositories;
using RedLife.Authorization.Users;
using RedLife.Core.Donations;
using RedLife.Core.UserBadges;
using System.Collections.Generic;
using System.Linq;

namespace RedLife.Core.Badges
{
    public class BadgeManager : IBadgeManager, ISingletonDependency
    {
        private readonly IRepository<Badge> _badgeRepository;
        private readonly IRepository<UserBadge> _userBadgeRepository;
        private readonly IRepository<Donation, string> _donationRepository;
        private readonly IRepository<User, long> _userRepository;

        public BadgeManager(IRepository<Badge> badgeRepository, 
            IRepository<UserBadge> userBadgeRepository,
            IRepository<Donation, string> donationRepository,
            IRepository<User, long> userRepository)
        {
            _badgeRepository = badgeRepository;
            _userBadgeRepository = userBadgeRepository;
            _donationRepository = donationRepository;
            _userRepository = userRepository;
        }
        public void AssignBadges(User user)
        {
            var alreadyAssignedBadges = GetAssignedBadges(user);

            var userDonations = _donationRepository
               .GetAll()
               .Where(x => x.DonorId == user.Id)
               .OrderByDescending(x => x.Date);

            CheckFirstDonationBadge(user, alreadyAssignedBadges, userDonations);
            CheckDonationAfterLongTimeBadge(user, alreadyAssignedBadges, userDonations);
            CheckDonationAfter3MonthsBadge(user, alreadyAssignedBadges, userDonations);
            CheckHolidayDonationBadge(user, alreadyAssignedBadges, userDonations);
            CheckCovidPlasmaDonationBadge(user, alreadyAssignedBadges, userDonations);
            CheckFirstSpecialDonationBadge(user, alreadyAssignedBadges, userDonations);
            Check3DonationsIn9MonthsBadge(user, alreadyAssignedBadges, userDonations);
        }

        public bool Check3DonationsIn9MonthsBadge(User user, ICollection<string> assignedBadges, IOrderedQueryable<Donation> userDonations)
        {
            if (!assignedBadges.Contains(BadgeTypes.ThreeDonationsIn9Months) && userDonations != null && userDonations.Count() >= 3)
            {
                if (userDonations != null && userDonations.Any())
                {
                    var mostRecentDonationDate = userDonations.FirstOrDefault().Date;
                    var secondMostRecentDonationDate = userDonations.ElementAt(1).Date;
                    var thirdMostRecentDonationDate = userDonations.ElementAt(2).Date;
                    var period1 = (mostRecentDonationDate - secondMostRecentDonationDate).Days;
                    var period2 = (secondMostRecentDonationDate - thirdMostRecentDonationDate).Days;

                    if (period1 >= 85 && period1 <= 100 && period2 >= 85 && period2 <= 100)
                    {
                        AssignBadgeToUser(BadgeTypes.ThreeDonationsIn9Months, user);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckFirstSpecialDonationBadge(User user, ICollection<string> assignedBadges, IOrderedQueryable<Donation> userDonations)
        {
            if (!assignedBadges.Contains(BadgeTypes.FirstSpecialDonation))
            {
                if (userDonations != null && userDonations.Any())
                {
                    var mostRecentDonationType = userDonations.FirstOrDefault().Type;
                    if (mostRecentDonationType == DonationTypes.SpecialDonation)
                    {
                        AssignBadgeToUser(BadgeTypes.FirstSpecialDonation, user);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckCovidPlasmaDonationBadge(User user, ICollection<string> assignedBadges, IOrderedQueryable<Donation> userDonations)
        {
            if (!assignedBadges.Contains(BadgeTypes.CovidPlasmaDonation))
            {
                if (userDonations != null && userDonations.Any())
                {
                    var mostRecentDonationType = userDonations.FirstOrDefault().Type;
                    if (mostRecentDonationType == DonationTypes.CovidPlasmaDonation)
                    {
                        AssignBadgeToUser(BadgeTypes.CovidPlasmaDonation, user);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckHolidayDonationBadge(User user, ICollection<string> assignedBadges, IOrderedQueryable<Donation> userDonations)
        {
            if (!assignedBadges.Contains(BadgeTypes.HolidayDonation))
            {
                if (userDonations != null && userDonations.Any())
                {
                    var mostRecentDonationDate = userDonations.FirstOrDefault().Date;
                    if (mostRecentDonationDate.Month == 12 || mostRecentDonationDate.Month == 1)
                    {
                        AssignBadgeToUser(BadgeTypes.HolidayDonation, user);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckDonationAfter3MonthsBadge(User user, ICollection<string> assignedBadges, IOrderedQueryable<Donation> userDonations)
        {
            if (!assignedBadges.Contains(BadgeTypes.DonationAfter3Months) && userDonations != null && userDonations.Count() >= 2)
            {
                if (userDonations != null && userDonations.Any())
                {
                    var mostRecentDonationDate = userDonations.FirstOrDefault().Date;
                    var secondMostRecentDonationDate = userDonations.ElementAt(1).Date;
                    var period = (mostRecentDonationDate - secondMostRecentDonationDate).TotalDays;
                    if (period >= 85 && period <= 100)
                    {
                        AssignBadgeToUser(BadgeTypes.DonationAfter3Months, user);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckDonationAfterLongTimeBadge(User user, ICollection<string> assignedBadges, IOrderedQueryable<Donation> userDonations)
        {
            if (!assignedBadges.Contains(BadgeTypes.DonationAfterLongTime) && userDonations != null && userDonations.Count() >= 2)
            {
                if (userDonations != null && userDonations.Any())
                {
                    var mostRecentDonationDate = userDonations.FirstOrDefault().Date;
                    var secondMostRecentDonationDate = userDonations.ElementAt(1).Date;
                    var period = (mostRecentDonationDate - secondMostRecentDonationDate).TotalDays;
                    if (period >= 365)
                    {
                        AssignBadgeToUser(BadgeTypes.DonationAfterLongTime, user);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckFirstDonationBadge(User user, ICollection<string> assignedBadges, IOrderedQueryable<Donation> userDonations)
        {
            if (!assignedBadges.Contains(BadgeTypes.FirstDonationBadge))
            {
                if (userDonations != null && userDonations.Count() == 1)
                {
                    AssignBadgeToUser(BadgeTypes.FirstDonationBadge, user);
                    return true;
                }
            }
            return false;
        }

        public void AssignBadgeToUser(string badgeName, User user)
        {
            var badge = _badgeRepository.FirstOrDefault(b => b.Name == badgeName);
            _userBadgeRepository.Insert(new UserBadge { BadgeId = badge.Id, UserId = user.Id });

            user.Points += badge.Points;
            _userRepository.Update(user);
        }

        public ICollection<string> GetAssignedBadges(User user)
        {
            ICollection<string> badgeNames = new List<string>();
            var userBadges = _userBadgeRepository.GetAll().Where(x => x.UserId == user.Id);
            if (userBadges != null && userBadges.Any())
            {
                foreach (var userBadge in userBadges)
                {
                    badgeNames.Add(_badgeRepository.Get(userBadge.BadgeId).Name);
                }
            }
            return badgeNames;
        }

        public ICollection<Badge> GetAll()
        {
            return _badgeRepository.GetAll().ToList();
        }

    }
}
