using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RedLife.Core.Appointments;
using RedLife.Core.LastId;
using RedLife.Authorization.Roles;
using RedLife.Authorization.Users;
using RedLife.MultiTenancy;

namespace RedLife.EntityFrameworkCore
{
    public class RedLifeDbContext : AbpZeroDbContext<Tenant, Role, User, RedLifeDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<LastUserId> LastUserId { get; set; }
        
        public RedLifeDbContext(DbContextOptions<RedLifeDbContext> options)
            : base(options)
        {
        }
    }
}
