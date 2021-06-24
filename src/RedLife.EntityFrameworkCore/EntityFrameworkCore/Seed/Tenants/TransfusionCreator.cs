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
            var donations = _context.Donations;
            var users = _context.Users;

            if (_context.Transfusions.Count() < 3)
            {
                var donor1 = _context.Users.FirstOrDefault(u => u.UserName == "madaist");
                var donor2 = _context.Users.FirstOrDefault(u => u.UserName == "andreiz");
                var donor3 = _context.Users.FirstOrDefault(u => u.UserName == "popescumaria");
                var donor4 = _context.Users.FirstOrDefault(u => u.UserName == "ion.g");

                var transfusion1 = new Transfusion
                {
                    Id = Guid.NewGuid().ToString(),
                    DonationId = donations.FirstOrDefault(d => d.DonorId == donor1.Id && d.Date == new DateTime(2021, 1, 12)).Id,
                    Date = new DateTime(2021, 1, 25),
                    HospitalId = users.FirstOrDefault(u => u.UserName == "adminElias").Id,
                    Quantity = 0.2
                };
                var transfusion2 = new Transfusion
                {
                    Id = Guid.NewGuid().ToString(),
                    DonationId = donations.FirstOrDefault(d => d.DonorId == donor1.Id && d.Date == new DateTime(2021, 1, 12)).Id,
                    Date = new DateTime(2021, 1, 29),
                    HospitalId = users.FirstOrDefault(u => u.UserName == "adminElias").Id,
                    Quantity = 0.2
                };
                var transfusion3 = new Transfusion
                {
                    Id = Guid.NewGuid().ToString(),
                    DonationId = donations.FirstOrDefault(d => d.DonorId == donor1.Id && d.Date == new DateTime(2021, 4, 15)).Id,
                    Date = new DateTime(2021, 4, 20),
                    HospitalId = users.FirstOrDefault(u => u.UserName == "adminElias").Id,
                    Quantity = 0.2
                };
                var transfusion4 = new Transfusion
                {
                    Id = Guid.NewGuid().ToString(),
                    DonationId = donations.FirstOrDefault(d => d.DonorId == donor1.Id && d.Date == new DateTime(2021, 4, 15)).Id,
                    Date = new DateTime(2021, 4, 17),
                    HospitalId = users.FirstOrDefault(u => u.UserName == "adminElias").Id,
                    Quantity = 0.2
                };
                var transfusion5 = new Transfusion
                {
                    Id = Guid.NewGuid().ToString(),
                    DonationId = donations.FirstOrDefault(d => d.DonorId == donor1.Id && d.Date == new DateTime(2020, 6, 1)).Id,
                    Date = new DateTime(2021, 6, 3),
                    HospitalId = users.FirstOrDefault(u => u.UserName == "adminElias").Id,
                    Quantity = 0.4
                };
                var transfusion6 = new Transfusion
                {
                    Id = Guid.NewGuid().ToString(),
                    DonationId = donations.FirstOrDefault(d => d.DonorId == donor2.Id && d.Date == new DateTime(2021, 2, 3)).Id,
                    Date = new DateTime(2021, 2, 6),
                    HospitalId = users.FirstOrDefault(u => u.UserName == "adminElias").Id,
                    Quantity = 0.15
                };
                var transfusion7 = new Transfusion
                {
                    Id = Guid.NewGuid().ToString(),
                    DonationId = donations.FirstOrDefault(d => d.DonorId == donor2.Id && d.Date == new DateTime(2021, 2, 3)).Id,
                    Date = new DateTime(2021, 2, 7),
                    HospitalId = users.FirstOrDefault(u => u.UserName == "adminElias").Id,
                    Quantity = 0.25
                };
                var transfusion8 = new Transfusion
                {
                    Id = Guid.NewGuid().ToString(),
                    DonationId = donations.FirstOrDefault(d => d.DonorId == donor3.Id && d.Date == new DateTime(2021, 2, 12)).Id,
                    Date = new DateTime(2021, 2, 15),
                    HospitalId = users.FirstOrDefault(u => u.UserName == "adminElias").Id,
                    Quantity = 0.2
                };
                var transfusion9 = new Transfusion
                {
                    Id = Guid.NewGuid().ToString(),
                    DonationId = donations.FirstOrDefault(d => d.DonorId == donor3.Id && d.Date == new DateTime(2021, 2, 12)).Id,
                    Date = new DateTime(2021, 2, 14),
                    HospitalId = users.FirstOrDefault(u => u.UserName == "adminElias").Id,
                    Quantity = 0.2
                };
                var transfusion10 = new Transfusion
                {
                    Id = Guid.NewGuid().ToString(),
                    DonationId = donations.FirstOrDefault(d => d.DonorId == donor3.Id && d.Date == new DateTime(2021, 5, 20)).Id,
                    Date = new DateTime(2021, 6, 1),
                    HospitalId = users.FirstOrDefault(u => u.UserName == "adminElias").Id,
                    Quantity = 0.2
                };
                var transfusion11 = new Transfusion
                {
                    Id = Guid.NewGuid().ToString(),
                    DonationId = donations.FirstOrDefault(d => d.DonorId == donor4.Id && d.Date == new DateTime(2021, 6, 2)).Id,
                    Date = new DateTime(2021, 6, 5),
                    HospitalId = users.FirstOrDefault(u => u.UserName == "adminElias").Id,
                    Quantity = 0.4
                };
                var transfusion12 = new Transfusion
                {
                    Id = Guid.NewGuid().ToString(),
                    DonationId = donations.FirstOrDefault(d => d.DonorId == donor4.Id && d.Date == new DateTime(2021, 3, 1)).Id,
                    Date = new DateTime(2021, 3, 5),
                    HospitalId = users.FirstOrDefault(u => u.UserName == "adminElias").Id,
                    Quantity = 0.4
                };
                
                _context.Transfusions.AddRange(new Transfusion[] { transfusion1, transfusion2, transfusion3,
                    transfusion4, transfusion5, transfusion6, transfusion7, transfusion8, transfusion9, transfusion10, transfusion11, transfusion12});
                _context.SaveChanges();
            }
        }
    }
}

