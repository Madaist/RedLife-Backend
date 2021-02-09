using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using RedLife.Core.Appointments;
using System;

namespace RedLife.Application.Appointments.Dto
{
    public class UpdateAppointmentDto : EntityDto<int>
    {
        public long DonorId { get; set; }
        public long CenterId { get; set; }
        public DateTime Date { get; set; }
    }
}
