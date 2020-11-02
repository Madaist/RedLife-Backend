using Abp.Application.Services;
using RedLife.MultiTenancy.Dto;

namespace RedLife.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

