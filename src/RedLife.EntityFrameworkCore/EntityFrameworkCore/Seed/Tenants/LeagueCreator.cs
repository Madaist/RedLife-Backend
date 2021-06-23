using RedLife.Core.Leagues;
using System.Linq;

namespace RedLife.EntityFrameworkCore.Seed.Tenants
{
    public class LeagueCreator
    {
        private readonly RedLifeDbContext _context;

        public LeagueCreator(RedLifeDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateLeagues();
        }

        private void CreateLeagues()
        {
            if (_context.Leagues.Count() < 3)
            {
                var league1 = new League
                {
                    Name = "Bronze",
                    Icon = "https://i.ibb.co/x2XhmKb/Badge-Bronze-Blank.png",
                    MinPoints = 0,
                    MaxPoints = 35
                };
                var league2 = new League
                {
                    Name = "Silver",
                    Icon = "https://i.ibb.co/N1WvGS7/Badge-Silver-Blank.png",
                    MinPoints = 36,
                    MaxPoints = 70
                };
                var league3 = new League
                {
                    Name = "Gold",
                    Icon = "https://i.ibb.co/ZKkccn9/Badge-Gold-Blank.png",
                    MinPoints = 71,
                    MaxPoints = 120
                };
                var league4 = new League
                {
                    Name = "Sapphire",
                    Icon = "https://i.ibb.co/vYB15Vr/Badge-Sapphire-Blank.png",
                    MinPoints = 121,
                    MaxPoints = 160
                };
                var league5 = new League
                {
                    Name = "Ruby",
                    Icon = "https://i.ibb.co/h1LK5dT/Badge-Ruby-Blank.png",
                    MinPoints = 161,
                    MaxPoints = 200
                };
                var league6 = new League
                {
                    Name = "Amethyst",
                    Icon = "https://i.ibb.co/cLtG5RC/Badge-Amethyst-Blank.png",
                    MinPoints = 201,
                    MaxPoints = 250
                };
                var league7 = new League
                {
                    Name = "Pearl",
                    Icon = "https://i.ibb.co/jMRMtRV/Badge-Pearl-Blank.png",
                    MinPoints = 251,
                    MaxPoints = 290
                };
                var league8 = new League
                {
                    Name = "Obsidian",
                    Icon = "https://i.ibb.co/kBdQq6p/Badge-Obsidian-Blank.png",
                    MinPoints = 291,
                    MaxPoints = 400
                };
                var league9 = new League
                {
                    Name = "Diamond",
                    Icon = "https://i.ibb.co/DMRB8k9/Badge-Diamond-Blank.png",
                    MinPoints = 401,
                    MaxPoints = 10000
                };

                _context.Leagues.AddRange(new League[] { league1, league2, league3, league4,
                 league5, league6, league7, league8, league9});
                _context.SaveChanges();
            }
        }
    }
}
