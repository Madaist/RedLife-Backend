using Abp.Application.Services.Dto;

namespace RedLife.Application.Transfusions.Dto
{
    public class TransfusionDto : EntityDto<string>
    {
        public string DonationId { get; set; }
        public string Date { get; set; }
        public long HospitalId { get; set; }
        public string HospitalName { get; set; }
        public double Quantity { get; set; }

    }
}
