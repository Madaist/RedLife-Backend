using System;
using System.Collections.Generic;
using System.Text;

namespace RedLife.Core.Statistics.AdminStatistics
{
    public interface IAdminStatisticsManager
    {
        public int GetNumberOfUsers();
        public int GetNumberOfDonations();
        public int GetNumberOfTransfusions();
        public int GetNumberOfAppointments();
        public List<int> GetDonationTypes();
        public List<int> GetDonationsPerMonth();
        public List<int> GetTransfusionsPerMonth();
    }
}
