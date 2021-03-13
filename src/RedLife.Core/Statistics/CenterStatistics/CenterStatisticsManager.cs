using Abp.Dependency;
using Abp.Domain.Repositories;
using RedLife.Authorization.Users;
using RedLife.Core.Appointments;
using RedLife.Core.Donations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedLife.Core.Statistics.CenterStatistics
{
    public class CenterStatisticsManager : ICenterStatisticsManager, ISingletonDependency
    {
        private readonly IRepository<Donation, string> _donationRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Appointment, int> _appointmentRepository;

        public CenterStatisticsManager(IRepository<Donation, string> donationRepository,
                                            IRepository<User, long> userRepository,
                                            IRepository<Appointment, int> appointmentRepository)
        {
            _donationRepository = donationRepository;
            _appointmentRepository = appointmentRepository;
            _userRepository = userRepository;
        }

        public int GetTotalDonationCount()
        {
            return _donationRepository.Count();
        }

        public int GetDonationCount(long centerId)
        {
            return _donationRepository
                .GetAll()
                .Where(x => x.CenterId == centerId)
                .Count();
        }

        public double GetDonationTotalQuantity(long centerId)
        {
            return Math.Round(_donationRepository
                .GetAll()
                .Where(x => x.CenterId == centerId)
                .Select(x => x.Quantity)
                .Sum(), 2);
        }

        public int GetAppointmentCount(long centerId)
        {
            return _appointmentRepository
                .GetAll()
                .Where(x => x.CenterId == centerId)
                .Count();

        }

        public int GetEmployeesCount(long centerId)
        {
            return _userRepository
                .GetAll()
                .Where(x => x.EmployerId == centerId)
                .Count();
        }

        public List<int> GetBloodTypesUsedCount(long centerId)
        {
            var donations = _donationRepository.GetAll()
              .Where(x => x.CenterId == centerId);

            var aPositive = GetNoOfSpecificBloodTypeDonation(donations, BloodTypes.APositive);
            var bPositive = GetNoOfSpecificBloodTypeDonation(donations, BloodTypes.BPositive);
            var cPositive = GetNoOfSpecificBloodTypeDonation(donations, BloodTypes.CPositive);
            var abPositive = GetNoOfSpecificBloodTypeDonation(donations, BloodTypes.ABPositive);
            var aNegative = GetNoOfSpecificBloodTypeDonation(donations, BloodTypes.ANegative);
            var bNegative = GetNoOfSpecificBloodTypeDonation(donations, BloodTypes.BNegative);
            var cNegative = GetNoOfSpecificBloodTypeDonation(donations, BloodTypes.CNegative);
            var abNegative = GetNoOfSpecificBloodTypeDonation(donations, BloodTypes.ABNegative);


            return new List<int> { aPositive, bPositive, cPositive, abPositive,
                                   aNegative, bNegative, cNegative, abNegative
            };
        }

        private int GetNoOfSpecificBloodTypeDonation(IQueryable<Donation> donations, string bloodType)
        {
            return donations
                .Where(x => x.BloodType == bloodType)
                .Count();
        }

        public List<int> GetDonationsPerMonth(long centerId)
        {
            List<int> donationsPerMonth = new List<int>();
            var donations = _donationRepository.GetAll()
              .Where(x => x.CenterId == centerId);

            var currentYear = DateTime.Now.Year;

            for (int i = 1; i <= 12; i++)
            {
                var monthDonations = donations
                    .Where(x => x.Date.Month == i && x.Date.Year == currentYear)
                    .Count();
                donationsPerMonth.Add(monthDonations);
            }

            return donationsPerMonth;
        }
    }
}
