using System.Threading.Tasks;
using Abp.Application.Services;
using RedLife.Authorization.Accounts.Dto;

namespace RedLife.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
