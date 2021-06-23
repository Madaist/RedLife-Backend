using RedLife.Core.Donations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedLife.EntityFrameworkCore.Seed.Tenants
{
    public class DonationInfoCreator
    {
        private readonly RedLifeDbContext _context;

        public DonationInfoCreator(RedLifeDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateDonationInfo();
        }

        private void CreateDonationInfo()
        {
            if (_context.DonationInfo.Count() < 3)
            {
                var donationInfo1 = new DonationInfo
                {
                    Id = DonationTypes.OrdinaryDonation,
                    Points = 20,
                    PeopleHelped = 3
                };
                var donationInfo2 = new DonationInfo
                {
                    Id = DonationTypes.SpecialDonation,
                    Points = 25,
                    PeopleHelped = 1
                };
                var donationInfo3 = new DonationInfo
                {
                    Id = DonationTypes.CovidPlasmaDonation,
                    Points = 35,
                    PeopleHelped = 3
                };

                _context.DonationInfo.AddRange(new DonationInfo[] { donationInfo1, donationInfo2, donationInfo3});
                _context.SaveChanges();
            }
        }
    }
}
