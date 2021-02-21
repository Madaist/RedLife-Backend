using Abp.Application.Services;
using RedLife.Application.Donations.Dto;

namespace RedLife.Application.Donations
{
    public interface IDonationAppService : IAsyncCrudAppService<DonationDto, string, PagedDonationResultRequestDto, CreateDonationDto, UpdateDonationDto>
    {
    }
}
