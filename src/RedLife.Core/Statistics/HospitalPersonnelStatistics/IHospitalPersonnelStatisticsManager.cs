using RedLife.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedLife.Core.Statistics.HospitalPersonnelStatistics
{
    public interface IHospitalPersonnelStatisticsManager
    {
        public int GetTotalTransfusionCount();
        public int GetTransfusionCount(User hospitalPersonnel);
        public double GetTransfusionTotalQuantity(User hospitalPersonnel);
        public int GetCovidTransfusionCount(User hospitalPersonnel);
        public int GetEmployeesCount(User hospitalPersonnel);
        public List<int> GetBloodTypesUsedCount(User hospitalPersonnel);
        public List<int> GetTransfusionsPerMonth(User hospitalPersonnel);
    }
}
