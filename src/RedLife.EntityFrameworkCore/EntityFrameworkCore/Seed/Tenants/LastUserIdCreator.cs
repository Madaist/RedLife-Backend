using Microsoft.EntityFrameworkCore;
using RedLife.Core.LastId;
using System.Linq;

namespace RedLife.EntityFrameworkCore.Seed.Tenants
{
    public class LastUserIdCreator
    {
        private readonly RedLifeDbContext _context;

        public LastUserIdCreator(RedLifeDbContext context)
        {
            _context = context;
        }


        public void Create()
        {
            CreateLastUserId();
        }

        public void CreateLastUserId()
        {
            var id = _context.LastUserId.IgnoreQueryFilters().FirstOrDefault();
            if (id == null)
            {
                _context.LastUserId.Add(new LastUserId() { Counter = 0 });
                _context.SaveChanges();
            }
        }
    }
}
