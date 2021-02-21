using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using RedLife.Core.Donations;
using System;
using System.ComponentModel.DataAnnotations;

namespace RedLife.Application.Donations.Dto
{
    [AutoMapTo(typeof(Donation))]
    public class CreateDonationDto : EntityDto<string>
    {
        [Required]
        public long DonorId { get; set; }
        [Required]
        public long CenterId { get; set; }
        [Required]
        public string Date { get; set; }
        public bool IsBloodAccepted { get; set; }
        public double Quantity { get; set; }
        public string BloodType { get; set; }
    }
}
