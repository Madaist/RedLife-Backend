using System.Threading.Tasks;
using Abp.Application.Services;
using RedLife.Sessions.Dto;

namespace RedLife.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
