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
        private readonly IRepository<DonationInfo, string> _donationInfoRepository;
        private readonly IRepository<Badge> _badgeRepository;
        private readonly IRepository<UserBadge> _userBadgesRepository;

        public AchievementsManager(IRepository<Donation, string> donationRepository,
            IRepository<DonationInfo, string> donationInfoRepository,
            IRepository<Badge> badgeRepository,
            IRepository<UserBadge> userBadgesRepository
            )
        {
            _donationRepository = donationRepository;
            _donationInfoRepository = donationInfoRepository;
            _badgeRepository = badgeRepository;
            _userBadgesRepository = userBadgesRepository;
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

    }
}
