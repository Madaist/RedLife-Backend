using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using RedLife.Authorization.Users;
using RedLife.Core.Donations;
using RedLife.Core.Transfusions;
using System;
using System.Collections.Generic;
using System.Linq;
using static RedLife.Authorization.Roles.StaticRoleNames;


namespace RedLife.Core.Statistics.DonorStatistics
{
    public class DonorStatisticsManager : IDonorStatisticsManager
    {
        private readonly UserManager _userManager;
        private readonly IRepository<Donation, string> _donationRepository;
        private readonly IRepository<Transfusion, string> _transfusionRepository;

        public DonorStatisticsManager(UserManager userManager, IRepository<Donation, string> donationRepository, IRepository<Transfusion, string> transfusionRepository)
        {
            _userManager = userManager;
            _donationRepository = donationRepository;
            _transfusionRepository = transfusionRepository;
        }



        public int GetNrOfDonations(long donorId)
        {
            return _donationRepository
                .GetAll()
                .Where(x => x.DonorId == donorId)
                .Count();
        }

        public int GetNrOfTransfusions(long donorId)
        {
            return _transfusionRepository
                .GetAll()
                .Where(x => x.Donation.DonorId == donorId)
                .Count();
        }

        public double GetDonatedQuantity(long donorId)
        {
            return Math.Round(_donationRepository.GetAll()
                .Where(x => x.DonorId == donorId)
                .Select(x => x.Quantity)
                .Sum(), 2);
        }

        public double GetTransfusionsQuantity(long donorId)
        {
            return Math.Round(_transfusionRepository.GetAll()
                .Where(x => x.Donation.DonorId == donorId)
                .Select(x => x.Quantity)
                .Sum(), 2);
        }

        public List<int> GetDonationTypes(long donorId)
        {
            var userDonations = _donationRepository.GetAll()
               .Where(x => x.DonorId == donorId);

            var ordinaryDonations = userDonations
                .Where(x => x.Type == DonationTypes.OrdinaryDonation)
                .Count();
            var specialDonations = userDonations
                .Where(x => x.Type == DonationTypes.SpecialDonation)
                .Count();
            var covidPlasmaDonations = userDonations
                .Where(x => x.Type == DonationTypes.CovidPlasmaDonation)
                .Count();

            return new List<int> { ordinaryDonations, specialDonations, covidPlasmaDonations };
        }

        public List<int> GetTransfusionsPerMonth(long donorId)
        {
            List<int> transfusionsPerMonth = new List<int>();
            var userTransfusions = _transfusionRepository.GetAll()
              .Where(x => x.Donation.DonorId == donorId);

            for (int i = 1; i <= 12; i++)
            {
                var monthTrasfusions = userTransfusions.Where(x => x.Date.Month == i).Count();
                transfusionsPerMonth.Add(monthTrasfusions);
            }

            return transfusionsPerMonth;
        }

        public int GetNrOfTotalAppTransfusions()
        {
            return _transfusionRepository.GetAll().Count();
        }
    }
}
