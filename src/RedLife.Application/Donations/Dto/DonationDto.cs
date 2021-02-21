using Abp.Application.Services.Dto;
using System;

namespace RedLife.Application.Donations.Dto
{
    public class DonationDto : EntityDto<string>
    {
        public string DonorName { get; set; }
        public string CenterName { get; set; }
        public long DonorId { get; set; }
        public long CenterId { get; set; }
        public String Date { get; set; }
        public bool IsBloodAccepted { get; set; }
        public double Quantity { get; set; }
        public string BloodType { get; set; }
    }
}
