using System.Collections.Generic;

namespace RedLife.Application.Statistics.Dto
{
    public class CenterStatisticsDto
    {
        public int DonationCount { get; set; }
        public double DonationTotalQuantity { get; set; }
        public int EmployeesCount { get; set; }
        public int AppointmentCount { get; set; }
        public int DonorCount { get; set; }
        public int HospitalCount { get; set; }
        public int CenterCount { get; set; }
        public int TotalDonationCount { get; set; }
        public List<int> BloodTypesUsedCount { get; set; }
        public List<int> DonationsPerMonth { get; set; }
    }
}
