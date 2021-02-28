using Abp.Application.Services.Dto;

namespace RedLife.Application.Transfusions.Dto
{
    public class PagedTransfusionResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}
