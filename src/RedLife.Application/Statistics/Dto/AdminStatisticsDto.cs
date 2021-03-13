using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace RedLife.Application.Statistics.Dto
{
    public class AdminStatisticsDto : EntityDto<string>
    {
        public int NumberOfDonors { get; set; }
        public int NumberOfHospitals { get; set; }
        public int NumberOfCenters { get; set; }
        public int NumberOfAdmins { get; set; }
        public int NumberOfUsers { get; set; }
        public int NumberOfDonations { get; set; }
        public int NumberOfTransfusions { get; set; }
        public int NumberOfAppointments { get; set; }
        public List<int> DonationTypes { get; set; }
        public List<int> DonationsPerMonth { get; set; }
        public List<int> TransfusionsPerMonth { get; set; }
        public List<int> RegisteredDonorsPerMonth { get; set; }
        public List<int> RegisteredCentersPerMonth { get; set; }
        public List<int> RegisteredHospitalsPerMonth { get; set; }
        
    }
}
