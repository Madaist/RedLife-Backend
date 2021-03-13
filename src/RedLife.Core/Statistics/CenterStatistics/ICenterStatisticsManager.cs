using System.Collections.Generic;

namespace RedLife.Core.Statistics.CenterStatistics
{
    public interface ICenterStatisticsManager
    {
        public int GetTotalDonationCount();
        public int GetDonationCount(long centerId);
        public double GetDonationTotalQuantity(long centerId);
        public int GetAppointmentCount(long centerId);
        public int GetEmployeesCount(long centerId);
        public List<int> GetBloodTypesUsedCount(long centerId);
        public List<int> GetDonationsPerMonth(long centerId);
    }
}
