﻿using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using RedLife.Core.Donations;
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
        public string Type { get; set; }
        public string MedicalTestsResult { get; set; }
        public string BloodReceiver { get; set; }
        public string HospitalReceiver { get; set; }
    }
}
