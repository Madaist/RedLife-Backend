using Abp.AutoMapper;
using RedLife.Core.Appointments;
using System;
using System.ComponentModel.DataAnnotations;

namespace RedLife.Application.Appointments.Dto
{
    [AutoMapTo(typeof(Appointment))]
    public class CreateAppointmentDto
    {
        [Required]
        public long DonorId { get; set; }
        [Required]
        public long CenterId { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
