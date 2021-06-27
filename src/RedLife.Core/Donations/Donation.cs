using Abp.Domain.Entities;
using RedLife.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RedLife.Core.Donations
{
    public class Donation : Entity<string>
    {
        public long DonorId { get; set; }
        public long CenterId { get; set; }
        public DateTime Date { get; set; }
        public bool IsBloodAccepted { get; set; }
        public double Quantity { get; set; }
        public string BloodType { get; set; }

        [ForeignKey("DonationInfo")]
        public string Type { get; set; }
        public string MedicalTestsResult { get; set; }
        public string BloodReceiver { get; set; }
        public string HospitalReceiver { get; set; }

        public virtual User Donor { get; set; }
        public virtual User Center { get; set; }
        public virtual DonationInfo DonationInfo { get; set; }
    }
}
