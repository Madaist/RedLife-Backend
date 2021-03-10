using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using RedLife.Application.Statistics.Dto;
using RedLife.Authorization;
using RedLife.Authorization.Users;
using RedLife.Core.Donations;
using RedLife.Core.Transfusions;
using System;
using System.Collections.Generic;
using System.Linq;
using static RedLife.Authorization.Roles.StaticRoleNames;

namespace RedLife.Application.Statistics
{
    public class StatisticsAppService :  IStatisticsAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly UserManager _userManager;
        private readonly IRepository<Donation, string> _donationRepository;
        private readonly IRepository<Transfusion, string> _transfusionRepository;
        public IAbpSession AbpSession { get; set; }

        public StatisticsAppService(IRepository<User, long> userRepository, UserManager userManager, IRepository<Donation, string> donationRepository, IRepository<Transfusion, string> transfusionRepository)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _donationRepository = donationRepository;
            _transfusionRepository = transfusionRepository;
            AbpSession = NullAbpSession.Instance;
        }

        [AbpAuthorize(PermissionNames.Donor)]
        public DonorStatisticsDto GetDonorStatistics()
        {
            DonorStatisticsDto statistics = new DonorStatisticsDto();
            var currentUser = _userRepository.Get(AbpSession.UserId?? 0);

            statistics.NumberOfDonations = _donationRepository
                .GetAll()
                .Where(x => x.DonorId == currentUser.Id)
                .Count();

            statistics.NumberOfTransfusions = _transfusionRepository
                .GetAll()
                .Where(x => x.Donation.DonorId == currentUser.Id)
                .Count();

            statistics.LitersDonated = Math.Round(_donationRepository.GetAll()
                .Where(x => x.DonorId == currentUser.Id)
                .Select(x => x.Quantity)
                .Sum(), 2);

            statistics.LitersUsedInTransfusions = Math.Round(_transfusionRepository.GetAll()
                .Where(x => x.Donation.DonorId == currentUser.Id)
                .Select(x => x.Quantity)
                .Sum(), 2);

            var userDonations = _donationRepository.GetAll()
                .Where(x => x.DonorId == currentUser.Id);
            var ordinaryDonations = userDonations
                .Where(x => x.Type == DonationTypes.OrdinaryDonation)
                .Count();
            var specialDonations = userDonations
                .Where(x => x.Type == DonationTypes.SpecialDonation)
                .Count();
            var covidPlasmaDonations = userDonations
                .Where(x => x.Type == DonationTypes.CovidPlasmaDonation)
                .Count();

            statistics.TypesOfDonations = new List<int> { ordinaryDonations, specialDonations, covidPlasmaDonations};


            statistics.TransfusionsPerMonth = new List<int>();
            var userTransfusions = _transfusionRepository.GetAll()
                .Where(x => x.Donation.DonorId == currentUser.Id);

            for (int i = 1; i <= 12; i++){
                var monthTrasfusions = userTransfusions.Where(x => x.Date.Month == i).Count();
                statistics.TransfusionsPerMonth.Add(monthTrasfusions);
            }

            statistics.NumberOfDonors = _userManager.GetUsersInRoleAsync(Tenants.Donor).Result.Count();
            statistics.NumberOfHospitals = _userManager.GetUsersInRoleAsync(Tenants.HospitalAdmin).Result.Count();
            statistics.NumberOfTransfusionCenters = _userManager.GetUsersInRoleAsync(Tenants.CenterAdmin).Result.Count();
            statistics.NumberOfAppTransfusions = _transfusionRepository.GetAll().Count();
            
            return statistics;
        }


    }
}
