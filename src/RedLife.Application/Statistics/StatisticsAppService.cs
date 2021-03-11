using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using RedLife.Application.Statistics.Dto;
using RedLife.Authorization;
using RedLife.Authorization.Users;
using RedLife.Core.Statistics.DonorStatistics;
using System.Linq;
using static RedLife.Authorization.Roles.StaticRoleNames;

namespace RedLife.Application.Statistics
{
    public class StatisticsAppService : IStatisticsAppService
    {

        public readonly IDonorStatisticsManager _donorStatisticsManager;
        private readonly UserManager _userManager;
        public IAbpSession AbpSession { get; set; }

        public StatisticsAppService(IDonorStatisticsManager donorStatisticsManager, UserManager userManager)
        {
            _donorStatisticsManager = donorStatisticsManager;
            _userManager = userManager;

            AbpSession = NullAbpSession.Instance;
        }

        [AbpAuthorize(PermissionNames.Donor)]
        [UnitOfWork]
        public DonorStatisticsDto GetDonorStatistics()
        {
            long donorId = AbpSession.UserId ?? 0;
            
            DonorStatisticsDto statistics = new DonorStatisticsDto
            {
                NumberOfDonations = _donorStatisticsManager.GetNrOfDonations(donorId),
                NumberOfTransfusions = _donorStatisticsManager.GetNrOfTransfusions(donorId),

                LitersDonated = _donorStatisticsManager.GetDonatedQuantity(donorId),
                LitersUsedInTransfusions = _donorStatisticsManager.GetTransfusionsQuantity(donorId),

                TypesOfDonations = _donorStatisticsManager.GetDonationTypes(donorId),
                TransfusionsPerMonth = _donorStatisticsManager.GetTransfusionsPerMonth(donorId),

                NumberOfDonors = _userManager.GetUsersInRoleAsync(Tenants.Donor).Result.Count(),
                NumberOfHospitals = _userManager.GetUsersInRoleAsync(Tenants.HospitalAdmin).Result.Count(),
                NumberOfTransfusionCenters = _userManager.GetUsersInRoleAsync(Tenants.CenterAdmin).Result.Count(),
                NumberOfAppTransfusions = _donorStatisticsManager.GetNrOfTotalAppTransfusions()
            };

            return statistics;
        }


    }
}
