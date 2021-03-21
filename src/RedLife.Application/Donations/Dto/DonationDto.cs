using Abp.Application.Services.Dto;
using System;

namespace RedLife.Application.Donations.Dto
{
    public class DonationDto : EntityDto<string>
    {
        public string DonorFirstName { get; set; }
        public string DonorLastName { get; set; }
        public string CenterName { get; set; }
        public long DonorId { get; set; }
        public long CenterId { get; set; }
        public string Date { get; set; }
        public bool IsBloodAccepted { get; set; }
        public double Quantity { get; set; }
        public string BloodType { get; set; }
        public string Type { get; set; }
        public string MedicalTestsResult { get; set; }
    }
}
