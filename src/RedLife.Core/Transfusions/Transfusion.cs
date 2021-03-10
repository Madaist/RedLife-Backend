using Abp.Domain.Entities;
using RedLife.Authorization.Users;
using RedLife.Core.Donations;
using System;

namespace RedLife.Core.Transfusions
{
    public class Transfusion : Entity<string>
    {
        public string DonationId { get; set; }
        public DateTime Date { get; set; }
        public long HospitalId { get; set; }
        public double Quantity { get; set; }

        public virtual Donation Donation { get; set; }
        public virtual User Hospital { get; set; }
    }
}
