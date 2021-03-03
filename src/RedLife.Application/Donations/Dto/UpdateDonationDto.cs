using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using RedLife.Core.Donations;
using System;

namespace RedLife.Application.Donations.Dto
{
    [AutoMapTo(typeof(Donation))]
    public class UpdateDonationDto : EntityDto<string>
    {
        public long DonorId { get; set; }
        public long CenterId { get; set; }
        public String Date { get; set; }
        public bool IsBloodAccepted { get; set; }
        public double Quantity { get; set; }
        public string BloodType { get; set; }
        public string Type { get; set; }
    }
}
