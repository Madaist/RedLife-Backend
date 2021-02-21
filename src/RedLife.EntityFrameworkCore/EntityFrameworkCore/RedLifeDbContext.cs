using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RedLife.Core.Appointments;
using RedLife.Core.LastId;
using RedLife.Authorization.Roles;
using RedLife.Authorization.Users;
using RedLife.MultiTenancy;
using RedLife.Core.Donations;

namespace RedLife.EntityFrameworkCore
{
    public class RedLifeDbContext : AbpZeroDbContext<Tenant, Role, User, RedLifeDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<LastUserId> LastUserId { get; set; }
        public DbSet<Donation> Donations { get; set; }
        
        public RedLifeDbContext(DbContextOptions<RedLifeDbContext> options)
            : base(options)
        {
        }
    }
}
