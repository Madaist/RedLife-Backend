using Abp.Application.Services.Dto;

namespace RedLife.Application.Appointments.Dto
{
    public class PagedAppointmentResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}
