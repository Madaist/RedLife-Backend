using Abp.Application.Services;
using RedLife.Application.Appointments.Dto;

namespace RedLife.Application.Appointments
{
    public interface IAppointmentAppService : IAsyncCrudAppService<AppointmentDto>
    {

    }
}
