using Abp.Application.Services;
using RedLife.Application.Statistics.Dto;

namespace RedLife.Application.Statistics
{
    public interface IStatisticsAppService : IApplicationService
    {
        public DonorStatisticsDto GetDonorStatistics();
    }
}
