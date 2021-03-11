using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace RedLife.Application.Statistics.Dto
{
    public class DonorStatisticsDto  : EntityDto<string>
    {
        //Statistics concerning the donor
        public int NumberOfDonations { get; set; }
        public double LitersDonated { get; set; }
        public int NumberOfTransfusions { get; set; }
        public double LitersUsedInTransfusions { get; set; }
        public List<int> TypesOfDonations { get; set; }
        public List<int> TransfusionsPerMonth { get; set; }

        // Statistics concerning the application
        public int NumberOfAppTransfusions { get; set; }
        public int NumberOfHospitals { get; set; }
        public int NumberOfTransfusionCenters { get; set; }
        public int NumberOfDonors { get; set; }
    }
}
