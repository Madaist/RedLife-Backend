using Abp.AutoMapper;
using RedLife.Core.Appointments;
using System;
using System.ComponentModel.DataAnnotations;

namespace RedLife.Application.Appointments.Dto
{
    [AutoMapTo(typeof(Appointment))]
    public class CreateAppointmentDto
    {
        public long DonorId { get; set; }
        public long CenterId { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
