using Abp.Domain.Entities;
using RedLife.Authorization.Users;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RedLife.Appointments
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
