using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace RedLife.Application.Appointments.Dto
{
    public class AppointmentDto : EntityDto<int>
    {
        public string DonorName { get; set; }
        public string CenterName { get; set; }
        public long DonorId { get; set; }
        public long CenterId { get; set; }
        public String Date { get; set; }
    }
}
