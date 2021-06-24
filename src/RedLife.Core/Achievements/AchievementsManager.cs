using Abp.Dependency;
using Abp.Domain.Repositories;
using RedLife.Authorization.Users;
using RedLife.Core.Badges;
using RedLife.Core.Donations;
using RedLife.Core.UserBadges;
using System.Collections.Generic;
using System.Linq;

namespace RedLife.Core.Achievements
{
    public class AchievementsManager : IAchievementsManager, ISingletonDependency
    {
        private readonly IRepository<Donation, string> _donationRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<DonationInfo, string> _donationInfoRepository;
        private readonly IRepository<Badge> _badgeRepository;
        private readonly IRepository<UserBadge> _userBadgesRepository;
        private readonly IBadgeManager _badgeManager;

        public AchievementsManager(IRepository<Donation, string> donationRepository,
            IRepository<DonationInfo, string> donationInfoRepository,
            IRepository<Badge> badgeRepository,
            IRepository<UserBadge> userBadgesRepository,
            IRepository<User, long> userRepository,
            IBadgeManager badgeManager
            )
        {
            _donationRepository = donationRepository;
            _donationInfoRepository = donationInfoRepository;
            _badgeRepository = badgeRepository;
            _userBadgesRepository = userBadgesRepository;
            _userRepository = userRepository;
            _badgeManager = badgeManager;
        }

        public int GetPeopleHelped(long donorId)
        {
            int peopleHelped = 0;

            var userDonations = _donationRepository.GetAll().Where(x => x.DonorId == donorId);
            if (userDonations != null && userDonations.Any())
            {
                foreach (Donation donation in userDonations)
                {
                    peopleHelped += _donationInfoRepository.Get(donation.Type).PeopleHelped;
                }
            }
            return peopleHelped;
        }

        public ICollection<Badge> GetUnassignedBadges(long donorId)
        {
            ICollection<Badge> unassignedBadges = new List<Badge>();
            var assignedBadgeIds = _userBadgesRepository.GetAll().Where(x => x.UserId == donorId).Select(x => x.BadgeId);
            var allPossibleBadges = _badgeRepository.GetAll().Select(x => x.Id);
            var unassignedBadgeIds = allPossibleBadges.Except(assignedBadgeIds);

            foreach (int badgeId in unassignedBadgeIds)
            {
                unassignedBadges.Add(_badgeRepository.Get(badgeId));
            }
            return unassignedBadges;
        }

        public ICollection<Badge> GetAssignedBadges(long donorId)
        {
            ICollection<Badge> assignedBadges = new List<Badge>();
            var userBadges = _userBadgesRepository.GetAll().Where(x => x.UserId == donorId);
            if (userBadges != null && userBadges.Any())
            {
                foreach (var userBadge in userBadges)
                {
                    assignedBadges.Add(_badgeRepository.Get(userBadge.BadgeId));
                }
            }
            return assignedBadges;
        }

        public void UpdateLeagueandBadges(User user)
        {
            _badgeManager.AssignBadges(user);
            UpdateLeague(user);
           
        }

        public void UpdateLeague(User user)
        {
            if (user.Points >= 0 && user.Points <= 35) user.LeagueId = 1;
            else if (user.Points > 35 && user.Points <= 70) user.LeagueId = 2;
            else if (user.Points > 70 && user.Points <= 120) user.LeagueId = 3;
            else if (user.Points > 120 && user.Points <= 160) user.LeagueId = 4;
            else if (user.Points > 160 && user.Points <= 200) user.LeagueId = 5;
            else if (user.Points > 200 && user.Points <= 250) user.LeagueId = 6;
            else if (user.Points > 250 && user.Points <= 290) user.LeagueId = 7;
            else if (user.Points > 290 && user.Points <= 400) user.LeagueId = 8;
            else if (user.Points > 400) user.LeagueId = 9;

            _userRepository.Update(user);
        }

    }
}
