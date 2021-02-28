using Microsoft.EntityFrameworkCore;
using RedLife.Authorization.Users;
using RedLife.Core.Donations;
using System;
using System.Linq;

namespace RedLife.EntityFrameworkCore.Seed.Tenants
{
    public class DonationCreator
    {
        private readonly RedLifeDbContext _context;

        public DonationCreator(RedLifeDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateDonations();
        }

        public void CreateDonations()
        {
            var centerAdminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.UserName == User.CenterAdminUserName);
            var donorUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.UserName == User.DonorUserName);

            if (_context.Donations.Count() < 3)
            {
                var donation1 = new Donation
                {
                    CenterId = centerAdminUser.Id,
                    DonorId = donorUser.Id,
                    Date = DateTime.UtcNow.AddDays(1),
                    IsBloodAccepted = true,
                    BloodType = "A",
                    Id = donorUser.Id.ToString() + "20210301",
                    Quantity = 0.4
                };
                var donation2 = new Donation
                {
                    CenterId = centerAdminUser.Id,
                    DonorId = donorUser.Id,
                    Date = DateTime.UtcNow.AddDays(2),
                    IsBloodAccepted = false,
                    BloodType = "A",
                    Id = donorUser.Id.ToString() + "20210401",
                    Quantity = 0.4
                };
                var donation3 = new Donation
                {
                    CenterId = centerAdminUser.Id,
                    DonorId = donorUser.Id,
                    Date = DateTime.UtcNow.AddDays(3),
                    IsBloodAccepted = true,
                    BloodType = "A",
                    Id = donorUser.Id.ToString() + "20210201",
                    Quantity = 0.4
                };
                _context.Donations.AddRange(new Donation[] { donation1, donation2, donation3 });
                _context.SaveChanges();
            }
        }
    }
}
