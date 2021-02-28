using Abp.Application.Services;
using RedLife.Application.Transfusions.Dto;

namespace RedLife.Application.Transfusions
{
    public interface ITransfusionAppService : IAsyncCrudAppService<TransfusionDto, string, PagedTransfusionResultRequestDto, CreateTransfusionDto, UpdateTransfusionDto>
    {
    }
}
