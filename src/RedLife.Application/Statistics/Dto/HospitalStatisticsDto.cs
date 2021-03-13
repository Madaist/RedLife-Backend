using Abp.Domain.Entities;
using System.Collections.Generic;

namespace RedLife.Application.Statistics.Dto
{
    public class HospitalStatisticsDto : Entity<string>
    {
        public int TransfusionCount { get; set; }
        public double TransfusionTotalQuantity { get; set; }
        public int EmployeesCount { get; set; }
        public int CovidTransfusionCount { get; set; }
        public int DonorCount { get; set; }
        public int HospitalCount { get; set; }
        public int CenterCount { get; set; }
        public int TotalTransfusionCount { get; set; }
        public List<int> BloodTypesUsedCount { get; set; }
        public List<int> TransfusionsPerMonth { get; set; }
    }
}
