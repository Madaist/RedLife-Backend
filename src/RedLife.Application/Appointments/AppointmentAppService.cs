using Abp.Application.Services;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using RedLife.Application.Appointments.Dto;
using RedLife.Authorization;
using RedLife.Core.Appointments;

namespace RedLife.Application.Appointments
{
    [AllowAnonymous]
    public class AppointmentAppService : AsyncCrudAppService<Appointment, AppointmentDto>, IAppointmentAppService
    {
        private readonly IRepository<Appointment> _appointmentRepository;

        public AppointmentAppService(IRepository<Appointment> appointmentRepository) : base(appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;

            CreatePermissionName = PermissionNames.Appointment_Create;
        }

        //[AbpAuthorize(PermissionNames.Appointment_Create)]
        //public int Getrandom()
        //{
        //    return 5;
        //}
    }
}
