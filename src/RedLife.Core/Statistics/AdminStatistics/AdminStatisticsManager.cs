using Abp.Dependency;
using Abp.Domain.Repositories;
using RedLife.Authorization.Users;
using RedLife.Core.Appointments;
using RedLife.Core.Donations;
using RedLife.Core.Transfusions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedLife.Core.Statistics.AdminStatistics
{
    public class AdminStatisticsManager : IAdminStatisticsManager, ISingletonDependency
    {
        private readonly IRepository<Donation, string> _donationRepository;
        private readonly IRepository<Transfusion, string> _transfusionRepository;
        private readonly IRepository<Appointment, int> _appointmentRepository;
        private readonly IRepository<User, long> _userRepository;

        public AdminStatisticsManager(IRepository<Donation, string> donationRepository, 
                                      IRepository<Transfusion, string> transfusionRepository,
                                      IRepository<Appointment, int> appointmentRepository,
                                      IRepository<User, long> userRepository
                                     )
        {
            _donationRepository = donationRepository;
            _transfusionRepository = transfusionRepository;
            _appointmentRepository = appointmentRepository;
            _userRepository = userRepository;
        }

        public int GetNumberOfUsers()
        {
            return _userRepository.Count();
        }

        public int GetNumberOfDonations()
        {
            return _donationRepository.Count();
        }

        public int GetNumberOfTransfusions()
        {
            return _transfusionRepository.Count();
        }

        public int GetNumberOfAppointments()
        {
            return _appointmentRepository.Count();
        }

        public List<int> GetDonationTypes()
        {
            var donations = _donationRepository.GetAll();

            var ordinaryDonations = donations
                .Where(x => x.Type == DonationTypes.OrdinaryDonation)
                .Count();
            var specialDonations = donations
                .Where(x => x.Type == DonationTypes.SpecialDonation)
                .Count();
            var covidPlasmaDonations = donations
                .Where(x => x.Type == DonationTypes.CovidPlasmaDonation)
                .Count();

            return new List<int> { ordinaryDonations, specialDonations, covidPlasmaDonations };
        }

        public List<int> GetDonationsPerMonth()
        {
            List<int> donationsPerMonth = new List<int>();
            var donations = _donationRepository.GetAll();
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

        public List<int> GetTransfusionsPerMonth()
        {
            List<int> transfusionsPerMonth = new List<int>();
            var transfusions = _transfusionRepository.GetAll();

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
