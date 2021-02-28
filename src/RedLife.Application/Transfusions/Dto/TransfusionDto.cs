using Abp.Application.Services.Dto;
using System;

namespace RedLife.Application.Transfusions.Dto
{
    public class TransfusionDto : EntityDto<string>
    {
        public string DonationId { get; set; }
        public string Date { get; set; }

    }
}
