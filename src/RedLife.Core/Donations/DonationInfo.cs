using Abp.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RedLife.Core.Donations
{
    public class DonationInfo : Entity<string>
    {
        public int Points { get; set; }
        public int PeopleHelped { get; set; }

        public virtual ICollection<Donation> Donations { get; set; }
    }
}
