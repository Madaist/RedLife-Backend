using Abp.Dependency;
using Abp.Domain.Repositories;
using RedLife.Authorization.Users;
using RedLife.Core.Donations;
using RedLife.Core.Transfusions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedLife.Core.Statistics.HospitalStatistics
{
    public class HospitalStatisticsManager : IHospitalStatisticsManager, ISingletonDependency
    {
        private readonly IRepository<Transfusion, string> _transfusionRepository;
        private readonly IRepository<User, long> _userRepository;

        public HospitalStatisticsManager(IRepository<Transfusion, string> transfusionRepository,
                                              IRepository<User, long> userRepository)
        {
            _transfusionRepository = transfusionRepository;
            _userRepository = userRepository;
        }

        public int GetTotalTransfusionCount()
        {
            return _transfusionRepository.GetAll().Count();
        }

        public int GetTransfusionCount(long hospitalId)
        {
            return _transfusionRepository
                .GetAll()
                .Where(x => x.HospitalId == hospitalId)
                .Count();
        }

        public double GetTransfusionTotalQuantity(long hospitalId)
        {
            return Math.Round(_transfusionRepository
                .GetAll()
                .Where(x => x.HospitalId == hospitalId)
                .Select(x => x.Quantity)
                .Sum(), 2);
        }

        public int GetCovidTransfusionCount(long hospitalId)
        {
            return _transfusionRepository
                .GetAll()
                .Where(x => x.HospitalId == hospitalId && x.Donation.Type == DonationTypes.CovidPlasmaDonation)
                .Count();

        }

        public int GetEmployeesCount(long hospitalId)
        {
            return _userRepository
                .GetAll()
                .Where(x => x.EmployerId == hospitalId)
                .Count();
        }

        public List<int> GetBloodTypesUsedCount(long hospitalId)
        {
            var transfusions = _transfusionRepository.GetAll()
              .Where(x => x.HospitalId == hospitalId);

            var aPositive = GetNoOfSpecificBloodTypeTransfusion(transfusions, BloodTypes.APositive);
            var bPositive = GetNoOfSpecificBloodTypeTransfusion(transfusions, BloodTypes.BPositive);
            var cPositive = GetNoOfSpecificBloodTypeTransfusion(transfusions, BloodTypes.CPositive);
            var abPositive = GetNoOfSpecificBloodTypeTransfusion(transfusions, BloodTypes.ABPositive);
            var aNegative = GetNoOfSpecificBloodTypeTransfusion(transfusions, BloodTypes.ANegative);
            var bNegative = GetNoOfSpecificBloodTypeTransfusion(transfusions, BloodTypes.BNegative);
            var cNegative = GetNoOfSpecificBloodTypeTransfusion(transfusions, BloodTypes.CNegative);
            var abNegative = GetNoOfSpecificBloodTypeTransfusion(transfusions, BloodTypes.ABNegative);


            return new List<int> { aPositive, bPositive, cPositive, abPositive,
                                   aNegative, bNegative, cNegative, abNegative
            };
        }

        private int GetNoOfSpecificBloodTypeTransfusion(IQueryable<Transfusion> transfusions, string bloodType)
        {
            return transfusions
                .Where(x => x.Donation.BloodType == bloodType)
                .Count();
        }

        public List<int> GetTransfusionsPerMonth(long hospitalId)
        {
            List<int> transfusionsPerMonth = new List<int>();
            var transfusions = _transfusionRepository.GetAll()
              .Where(x => x.HospitalId == hospitalId);

            var currentYear = DateTime.Now.Year;

            for (int i = 1; i <= 12; i++)
            {
                var monthTrasfusions = transfusions
                    .Where(x => x.Date.Month == i && x.Date.Year == currentYear)
                    .Count();
                transfusionsPerMonth.Add(monthTrasfusions);
            }

            return transfusionsPerMonth;
        }


    }
}
