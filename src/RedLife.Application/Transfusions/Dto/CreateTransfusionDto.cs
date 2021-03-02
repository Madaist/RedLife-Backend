using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using RedLife.Core.Transfusions;
using System;
using System.ComponentModel.DataAnnotations;

namespace RedLife.Application.Transfusions.Dto
{
    [AutoMapTo(typeof(Transfusion))]
    public class CreateTransfusionDto : EntityDto<string>
    {
        [Required]
        public string DonationId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public long HospitalId { get; set; }
    }
}
