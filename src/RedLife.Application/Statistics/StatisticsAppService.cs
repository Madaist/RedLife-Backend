using Abp.Authorization;
using Abp.Runtime.Session;
using RedLife.Application.Statistics.Dto;
using RedLife.Authorization;
using RedLife.Authorization.Users;
using RedLife.Core.Statistics.AdminStatistics;
using RedLife.Core.Statistics.CenterStatistics;
using RedLife.Core.Statistics.DonorStatistics;
using RedLife.Core.Statistics.HospitalStatistics;
using System;
using System.Collections.Generic;
using System.Linq;
using static RedLife.Authorization.Roles.StaticRoleNames;

namespace RedLife.Application.Statistics
{
    [AbpAuthorize]
    public class StatisticsAppService : IStatisticsAppService
    {

        private readonly IDonorStatisticsManager _donorStatisticsManager;
        private readonly IAdminStatisticsManager _adminStatisticsManager;
        private readonly IHospitalStatisticsManager _hospitalStatisticsManager;
        private readonly ICenterStatisticsManager _centerStatisticsManager;
        private readonly UserManager _userManager;
        public IAbpSession AbpSession { get; set; }

        public StatisticsAppService(IDonorStatisticsManager donorStatisticsManager,
                                    IAdminStatisticsManager adminStatisticsManager,
                                    IHospitalStatisticsManager hospitalAdminStatisticsManager,
                                    ICenterStatisticsManager centerStatisticsManager,
                                    UserManager userManager)
        {
            _donorStatisticsManager = donorStatisticsManager;
            _adminStatisticsManager = adminStatisticsManager;
            _hospitalStatisticsManager = hospitalAdminStatisticsManager;
            _centerStatisticsManager = centerStatisticsManager;
            _userManager = userManager;

            AbpSession = NullAbpSession.Instance;
        }

        [AbpAuthorize(PermissionNames.Donor)]
        public DonorStatisticsDto GetDonorStatistics()
        {
            long donorId = AbpSession.UserId ?? 0;
            DonorStatisticsDto statistics = ComputeDonorStatistics(donorId);
            return statistics;
        }

        [AbpAuthorize(PermissionNames.Admin)]
        public AdminStatisticsDto GetAdminStatistics()
        {
            AdminStatisticsDto statistics = ComputeAdminStatistics();
            return statistics;
        }

        public HospitalStatisticsDto GetHospitalStatistics()
        {
            long userId = AbpSession.UserId ?? 0;
            User user = _userManager.GetUserById(userId);
            var userRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault();

            HospitalStatisticsDto statistics;
            if (userRole == Tenants.HospitalAdmin)
            {
                statistics = ComputeHospitalStatistics(userId);
            }
            else if (userRole == Tenants.HospitalPersonnel)
            {
                statistics = ComputeHospitalStatistics(user.EmployerId ?? 0);
            }
            else
            {
                throw new Exception("User not authorized to access hospital statistics");
            }

            return statistics;
        }

        public CenterStatisticsDto GetCenterStatistics()
        {
            long userId = AbpSession.UserId ?? 0;
            User user = _userManager.GetUserById(userId);
            var userRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault();

            CenterStatisticsDto statistics;
            if (userRole == Tenants.CenterAdmin)
            {
                statistics = ComputeCenterStatistics(userId);
            }
            else if (userRole == Tenants.CenterPersonnel)
            {
                statistics = ComputeCenterStatistics(user.EmployerId ?? 0);
            }
            else
            {
                throw new Exception("User not authorized to access center statistics");
            }

            return statistics;
        }


        private CenterStatisticsDto ComputeCenterStatistics(long userId)
        {
            return new CenterStatisticsDto
            {
                DonorCount = _userManager.GetUsersInRoleAsync(Tenants.Donor).Result.Count(),
                HospitalCount = _userManager.GetUsersInRoleAsync(Tenants.HospitalAdmin).Result.Count(),
                CenterCount = _userManager.GetUsersInRoleAsync(Tenants.CenterAdmin).Result.Count(),

                DonationCount = _centerStatisticsManager.GetDonationCount(userId),
                AppointmentCount = _centerStatisticsManager.GetAppointmentCount(userId),
                DonationTotalQuantity = _centerStatisticsManager.GetDonationTotalQuantity(userId),

                DonationsPerMonth = _centerStatisticsManager.GetDonationsPerMonth(userId),
                BloodTypesUsedCount = _centerStatisticsManager.GetBloodTypesUsedCount(userId),
                TotalDonationCount = _centerStatisticsManager.GetTotalDonationCount(),
                EmployeesCount = _centerStatisticsManager.GetEmployeesCount(userId)

            };
        }


        private HospitalStatisticsDto ComputeHospitalStatistics(long userId)
        {
            return new HospitalStatisticsDto
            {
                DonorCount = _userManager.GetUsersInRoleAsync(Tenants.Donor).Result.Count(),
                HospitalCount = _userManager.GetUsersInRoleAsync(Tenants.HospitalAdmin).Result.Count(),
                CenterCount = _userManager.GetUsersInRoleAsync(Tenants.CenterAdmin).Result.Count(),
                TotalTransfusionCount = _hospitalStatisticsManager.GetTotalTransfusionCount(),

                TransfusionCount = _hospitalStatisticsManager.GetTransfusionCount(userId),
                TransfusionTotalQuantity = _hospitalStatisticsManager.GetTransfusionTotalQuantity(userId),
                CovidTransfusionCount = _hospitalStatisticsManager.GetCovidTransfusionCount(userId),
                EmployeesCount = _hospitalStatisticsManager.GetEmployeesCount(userId),

                BloodTypesUsedCount = _hospitalStatisticsManager.GetBloodTypesUsedCount(userId),
                TransfusionsPerMonth = _hospitalStatisticsManager.GetTransfusionsPerMonth(userId)
            };
        }

        private DonorStatisticsDto ComputeDonorStatistics(long donorId)
        {
            return new DonorStatisticsDto
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
        }

        private AdminStatisticsDto ComputeAdminStatistics()
        {
            return new AdminStatisticsDto
            {
                NumberOfDonors = _userManager.GetUsersInRoleAsync(Tenants.Donor).Result.Count(),
                NumberOfHospitals = _userManager.GetUsersInRoleAsync(Tenants.HospitalAdmin).Result.Count(),
                NumberOfCenters = _userManager.GetUsersInRoleAsync(Tenants.CenterAdmin).Result.Count(),
                NumberOfAdmins = _userManager.GetUsersInRoleAsync(Tenants.Admin).Result.Count(),
                NumberOfUsers = _adminStatisticsManager.GetNumberOfUsers(),

                NumberOfAppointments = _adminStatisticsManager.GetNumberOfAppointments(),
                NumberOfDonations = _adminStatisticsManager.GetNumberOfDonations(),
                NumberOfTransfusions = _adminStatisticsManager.GetNumberOfTransfusions(),

                DonationTypes = _adminStatisticsManager.GetDonationTypes(),
                DonationsPerMonth = _adminStatisticsManager.GetDonationsPerMonth(),
                TransfusionsPerMonth = _adminStatisticsManager.GetTransfusionsPerMonth(),
                RegisteredDonorsPerMonth = GetRegisteredUsersPerMonth(Tenants.Donor),
                RegisteredCentersPerMonth = GetRegisteredUsersPerMonth(Tenants.CenterAdmin),
                RegisteredHospitalsPerMonth = GetRegisteredUsersPerMonth(Tenants.HospitalAdmin)
            };
        }

        private List<int> GetRegisteredUsersPerMonth(string role)
        {
            var users = _userManager.GetUsersInRoleAsync(role).Result;
            var currentYear = DateTime.Now.Year;
            List<int> usersPerMonth = new List<int>();
            for (int i = 1; i <= 12; i++)
            {
                var usersInMonth = users
                    .Where(x => x.CreationTime.Month == i && x.CreationTime.Year == currentYear)
                    .Count();
                usersPerMonth.Add(usersInMonth);
            }

            return usersPerMonth;
        }


    }
}
