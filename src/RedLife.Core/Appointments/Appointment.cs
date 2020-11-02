using Abp.Domain.Entities;
using RedLife.Authorization.Users;
using System;

namespace RedLife.Core.Appointments
{
    public class Appointment : Entity<int>
    {
        public long DonorId { get; set; }
        public long CenterId { get; set; }
        public DateTime Date { get; set; }

        public virtual User Donor { get; set; }

        public virtual User Center { get; set; }
    }
}
