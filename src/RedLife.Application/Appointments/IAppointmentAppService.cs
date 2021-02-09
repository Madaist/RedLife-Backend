using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RedLife.Application.Appointments.Dto;

namespace RedLife.Application.Appointments
{
    public interface IAppointmentAppService : IAsyncCrudAppService<AppointmentDto, int, PagedAppointmentResultRequestDto, CreateAppointmentDto, UpdateAppointmentDto>
    {

    }
}
