using System.Collections.Generic;

namespace RedLife.Core.Statistics.HospitalStatistics
{
    public interface IHospitalStatisticsManager
    {
        public int GetTotalTransfusionCount();
        public int GetTransfusionCount(long hospitalId);
        public double GetTransfusionTotalQuantity(long hospitalId);
        public int GetCovidTransfusionCount(long hospitalId);
        public int GetEmployeesCount(long hospitalId);
        public List<int> GetBloodTypesUsedCount(long hospitalId);
        public List<int> GetTransfusionsPerMonth(long hospitalId);
    }
}
