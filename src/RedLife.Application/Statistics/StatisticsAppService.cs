using Abp.Authorization;
using Abp.Runtime.Session;
using RedLife.Application.Statistics.Dto;
using RedLife.Authorization;
using RedLife.Authorization.Users;
using RedLife.Core.Statistics.AdminStatistics;
using RedLife.Core.Statistics.DonorStatistics;
using RedLife.Core.Statistics.HospitalAdminStatistics;
using RedLife.Core.Statistics.HospitalPersonnelStatistics;
using System;
using System.Collections.Generic;
using System.Linq;
using static RedLife.Authorization.Roles.StaticRoleNames;

namespace RedLife.Application.Statistics
{
    public class StatisticsAppService : IStatisticsAppService
    {

        private readonly IDonorStatisticsManager _donorStatisticsManager;
        private readonly IAdminStatisticsManager _adminStatisticsManager;
        private readonly IHospitalAdminStatisticsManager _hospitalAdminStatisticsManager;
        private readonly IHospitalPersonnelStatisticsManager _hospitalPersonnelStatisticsManager;
        private readonly UserManager _userManager;
        public IAbpSession AbpSession { get; set; }

        public StatisticsAppService(IDonorStatisticsManager donorStatisticsManager,
                                    IAdminStatisticsManager adminStatisticsManager,
                                    IHospitalAdminStatisticsManager hospitalAdminStatisticsManager,
                                    IHospitalPersonnelStatisticsManager hospitalPersonnelStatisticsManager,
                                    UserManager userManager)
        {
            _donorStatisticsManager = donorStatisticsManager;
            _adminStatisticsManager = adminStatisticsManager;
            _hospitalAdminStatisticsManager = hospitalAdminStatisticsManager;
            _hospitalPersonnelStatisticsManager = hospitalPersonnelStatisticsManager;
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
                statistics = ComputeHospitalAdminStatistics(userId);
            }
            else if (userRole == Tenants.HospitalPersonnel)
            {
                statistics = ComputeHospitalPersonnelStatistics(userId);
            }
            else
            {
                throw new Exception("User not authorized to access hospital statistics");
            }

            return statistics;
        }

        private HospitalStatisticsDto ComputeHospitalPersonnelStatistics(long userId)
        {
            User hospitalPersonnel = _userManager.GetUserById(userId);
            return new HospitalStatisticsDto
            {
                DonorCount = _userManager.GetUsersInRoleAsync(Tenants.Donor).Result.Count(),
                HospitalCount = _userManager.GetUsersInRoleAsync(Tenants.HospitalAdmin).Result.Count(),
                CenterCount = _userManager.GetUsersInRoleAsync(Tenants.CenterAdmin).Result.Count(),
                TotalTransfusionCount = _hospitalPersonnelStatisticsManager.GetTotalTransfusionCount(),

                TransfusionCount = _hospitalPersonnelStatisticsManager.GetTransfusionCount(hospitalPersonnel),
                TransfusionTotalQuantity = _hospitalPersonnelStatisticsManager.GetTransfusionTotalQuantity(hospitalPersonnel),
                CovidTransfusionCount = _hospitalPersonnelStatisticsManager.GetCovidTransfusionCount(hospitalPersonnel),
                EmployeesCount = _hospitalPersonnelStatisticsManager.GetEmployeesCount(hospitalPersonnel),

                BloodTypesUsedCount = _hospitalPersonnelStatisticsManager.GetBloodTypesUsedCount(hospitalPersonnel),
                TransfusionsPerMonth = _hospitalPersonnelStatisticsManager.GetTransfusionsPerMonth(hospitalPersonnel)
            };
        }

        private HospitalStatisticsDto ComputeHospitalAdminStatistics(long userId)
        {
            return new HospitalStatisticsDto
            {
                DonorCount = _userManager.GetUsersInRoleAsync(Tenants.Donor).Result.Count(),
                HospitalCount = _userManager.GetUsersInRoleAsync(Tenants.HospitalAdmin).Result.Count(),
                CenterCount = _userManager.GetUsersInRoleAsync(Tenants.CenterAdmin).Result.Count(),
                TotalTransfusionCount = _hospitalAdminStatisticsManager.GetTotalTransfusionCount(),

                TransfusionCount = _hospitalAdminStatisticsManager.GetTransfusionCount(userId),
                TransfusionTotalQuantity = _hospitalAdminStatisticsManager.GetTransfusionTotalQuantity(userId),
                CovidTransfusionCount = _hospitalAdminStatisticsManager.GetCovidTransfusionCount(userId),
                EmployeesCount = _hospitalAdminStatisticsManager.GetEmployeesCount(userId),

                BloodTypesUsedCount = _hospitalAdminStatisticsManager.GetBloodTypesUsedCount(userId),
                TransfusionsPerMonth = _hospitalAdminStatisticsManager.GetTransfusionsPerMonth(userId)
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
