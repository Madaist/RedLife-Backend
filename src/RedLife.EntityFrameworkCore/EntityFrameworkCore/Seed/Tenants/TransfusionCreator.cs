using Microsoft.EntityFrameworkCore;
using RedLife.Authorization.Users;
using RedLife.Core.Transfusions;
using System;
using System.Linq;

namespace RedLife.EntityFrameworkCore.Seed.Tenants
{
    public class TransfusionCreator
    {
        private readonly RedLifeDbContext _context;

        public TransfusionCreator(RedLifeDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateTransfusions();
        }

        private void CreateTransfusions()
        {
            var donation = _context.Donations.IgnoreQueryFilters().FirstOrDefault();
            var hospitalUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.UserName == User.HospitalAdminUserName);

            if (_context.Transfusions.Count() < 3)
            {
                var transfusion1 = new Transfusion
                {
                    Id = Guid.NewGuid().ToString(),
                    DonationId = donation.Id,
                    Date = DateTime.UtcNow.AddDays(1),
                    HospitalId = hospitalUser.Id
                };
                var transfusion2 = new Transfusion
                {
                    Id = Guid.NewGuid().ToString(),
                    DonationId = donation.Id,
                    Date = DateTime.UtcNow.AddDays(2),
                    HospitalId = hospitalUser.Id
                };
                var transfusion3 = new Transfusion
                {
                    Id = Guid.NewGuid().ToString(),
                    DonationId = donation.Id,
                    Date = DateTime.UtcNow.AddDays(3),
                    HospitalId = hospitalUser.Id
                };
                _context.Transfusions.AddRange(new Transfusion[] { transfusion1, transfusion2, transfusion3 });
                _context.SaveChanges();
            }
        }
    }
}

