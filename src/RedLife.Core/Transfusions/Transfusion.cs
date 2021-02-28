using Abp.Domain.Entities;
using RedLife.Core.Donations;
using System;

namespace RedLife.Core.Transfusions
{
    public class Transfusion : Entity<string>
    {
        public string DonationId { get; set; }
        public DateTime Date { get; set; }

        public virtual Donation Donation { get; set; }
    }
}
