using Microsoft.EntityFrameworkCore;
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
          
            if (_context.Transfusions.Count() < 3)
            {
                var transfusion1 = new Transfusion
                {
                    Id = Guid.NewGuid().ToString(),
                    DonationId = donation.Id,
                    Date = DateTime.UtcNow.AddDays(1)
                };
                var transfusion2 = new Transfusion
                {
                    Id = Guid.NewGuid().ToString(),
                    DonationId = donation.Id,
                    Date = DateTime.UtcNow.AddDays(2)
                };
                var transfusion3 = new Transfusion
                {
                    Id = Guid.NewGuid().ToString(),
                    DonationId = donation.Id,
                    Date = DateTime.UtcNow.AddDays(3)
                };
                _context.Transfusions.AddRange(new Transfusion[] { transfusion1, transfusion2, transfusion3 });
                _context.SaveChanges();
            }
        }
    }
}

