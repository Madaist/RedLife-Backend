using RedLife.Core.LastId;
using RedLife.EntityFrameworkCore.Seed.Tenants;

namespace RedLife.EntityFrameworkCore.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly RedLifeDbContext _context;

        public InitialHostDbBuilder(RedLifeDbContext context)
        {
            _context = context;
        }

        [System.Obsolete]
        public void Create()
        {
            new LeagueCreator(_context).Create();
            new LastUserIdCreator(_context).Create();
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
