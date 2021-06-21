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
            var users = _context.Users;

            if (_context.Donations.Count() < 3)
            {
                var donation1 = new Donation
                {
                    CenterId = users.FirstOrDefault(u => u.UserName == "adminReginaMaria").Id,
                    DonorId = users.FirstOrDefault(u => u.UserName == "madaist").Id,
                    Date = new DateTime(2021, 1, 12),
                    IsBloodAccepted = true,
                    BloodType = BloodTypes.APositive,
                    Id = users.FirstOrDefault(u => u.UserName == "madaist").Id.ToString() + "20210112",
                    Quantity = 0.4,
                    Type = DonationTypes.OrdinaryDonation
                };
                var donation2 = new Donation
                {
                    CenterId = users.FirstOrDefault(u => u.UserName == "adminReginaMaria").Id,
                    DonorId = users.FirstOrDefault(u => u.UserName == "madaist").Id,
                    Date = new DateTime(2021, 4, 15),
                    IsBloodAccepted = false,
                    BloodType = BloodTypes.APositive,
                    Id = users.FirstOrDefault(u => u.UserName == "madaist").Id.ToString() + "20210415",
                    Quantity = 0.4,
                    Type = DonationTypes.CovidPlasmaDonation
                };
                var donation3 = new Donation
                {
                    CenterId = users.FirstOrDefault(u => u.UserName == "adminCTS").Id,
                    DonorId = users.FirstOrDefault(u => u.UserName == "madaist").Id,
                    Date = new DateTime(2020, 6, 1),
                    IsBloodAccepted = true,
                    BloodType = BloodTypes.APositive,
                    Id = users.FirstOrDefault(u => u.UserName == "madaist").Id.ToString() + "20200601",
                    Quantity = 0.4,
                    Type = DonationTypes.SpecialDonation
                };
                var donation4 = new Donation
                {
                    CenterId = users.FirstOrDefault(u => u.UserName == "adminSinevo").Id,
                    DonorId = users.FirstOrDefault(u => u.UserName == "madaist").Id,
                    Date = new DateTime(2020, 2, 20),
                    IsBloodAccepted = true,
                    BloodType = BloodTypes.APositive,
                    Id = users.FirstOrDefault(u => u.UserName == "madaist").Id.ToString() + "20202620",
                    Quantity = 0.4,
                    Type = DonationTypes.OrdinaryDonation
                };
                var donation5 = new Donation
                {
                    CenterId = users.FirstOrDefault(u => u.UserName == "adminReginaMaria").Id,
                    DonorId = users.FirstOrDefault(u => u.UserName == "andreiz").Id,
                    Date = new DateTime(2021, 2, 3),
                    IsBloodAccepted = true,
                    BloodType = BloodTypes.BNegative,
                    Id = users.FirstOrDefault(u => u.UserName == "andreiz").Id.ToString() + "20210203",
                    Quantity = 0.4,
                    Type = DonationTypes.OrdinaryDonation
                };
                var donation6 = new Donation
                {
                    CenterId = users.FirstOrDefault(u => u.UserName == "adminReginaMaria").Id,
                    DonorId = users.FirstOrDefault(u => u.UserName == "popescumaria").Id,
                    Date = new DateTime(2021, 2, 12),
                    IsBloodAccepted = true,
                    BloodType = BloodTypes.CPositive,
                    Id = users.FirstOrDefault(u => u.UserName == "popescumaria").Id.ToString() + "20210212",
                    Quantity = 0.4,
                    Type = DonationTypes.OrdinaryDonation
                };
                var donation7 = new Donation
                {
                    CenterId = users.FirstOrDefault(u => u.UserName == "adminReginaMaria").Id,
                    DonorId = users.FirstOrDefault(u => u.UserName == "popescumaria").Id,
                    Date = new DateTime(2021, 5, 20),
                    IsBloodAccepted = true,
                    BloodType = BloodTypes.CPositive,
                    Id = users.FirstOrDefault(u => u.UserName == "popescumaria").Id.ToString() + "20210520",
                    Quantity = 0.4,
                    Type = DonationTypes.SpecialDonation
                };
                var donation8 = new Donation
                {
                    CenterId = users.FirstOrDefault(u => u.UserName == "adminReginaMaria").Id,
                    DonorId = users.FirstOrDefault(u => u.UserName == "ion.g").Id,
                    Date = new DateTime(2021, 6, 2),
                    IsBloodAccepted = true,
                    BloodType = BloodTypes.ABNegative,
                    Id = users.FirstOrDefault(u => u.UserName == "ion.g").Id.ToString() + "20210602",
                    Quantity = 0.4,
                    Type = DonationTypes.CovidPlasmaDonation
                };
                var donation9 = new Donation
                {
                    CenterId = users.FirstOrDefault(u => u.UserName == "adminReginaMaria").Id,
                    DonorId = users.FirstOrDefault(u => u.UserName == "ion.g").Id,
                    Date = new DateTime(2021, 3, 1),
                    IsBloodAccepted = true,
                    BloodType = BloodTypes.ABNegative,
                    Id = users.FirstOrDefault(u => u.UserName == "ion.g").Id.ToString() + "20210301",
                    Quantity = 0.4,
                    Type = DonationTypes.SpecialDonation
                };
                var donation10 = new Donation
                {
                    CenterId = users.FirstOrDefault(u => u.UserName == "adminReginaMaria").Id,
                    DonorId = users.FirstOrDefault(u => u.UserName == "gabimusat").Id,
                    Date = new DateTime(2021, 6, 15),
                    IsBloodAccepted = true,
                    BloodType = BloodTypes.APositive,
                    Id = users.FirstOrDefault(u => u.UserName == "gabimusat").Id.ToString() + "20210615",
                    Quantity = 0.4,
                    Type = DonationTypes.OrdinaryDonation
                };

                _context.Donations.AddRange(new Donation[] { donation1, donation2, donation3, donation4,
                    donation5, donation6, donation7, donation8, donation9, donation10});
                _context.SaveChanges();
            }
        }
    }
}
