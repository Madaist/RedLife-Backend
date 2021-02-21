using Abp.Application.Services.Dto;

namespace RedLife.Application.Donations.Dto
{
    public class PagedDonationResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}
