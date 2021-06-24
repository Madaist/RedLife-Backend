using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using RedLife.EntityFrameworkCore.Seed.Host;
using RedLife.EntityFrameworkCore.Seed.Tenants;
using System;
using System.Transactions;

namespace RedLife.EntityFrameworkCore.Seed
{
    public static class SeedHelper
    {
        [Obsolete]
        public static void SeedHostDb(IIocResolver iocResolver)
        {
            WithDbContext<RedLifeDbContext>(iocResolver, SeedHostDb);
        }


        [Obsolete]
        public static void SeedHostDb(RedLifeDbContext context)
        {
            context.SuppressAutoSetTenantId = true;

            new LeagueCreator(context).Create();
            new BadgeCreator(context).Create();
            new DonationInfoCreator(context).Create();
            new InitialHostDbBuilder(context).Create();
            new DefaultTenantBuilder(context).Create();
            new TenantRoleAndUserBuilder(context, 1).Create();
            new DonationCreator(context).Create();
            new AppointmentCreator(context).Create();
            new TransfusionCreator(context).Create();
            new UserBadgesCreator(context).Create();
        }

        private static void WithDbContext<TDbContext>(IIocResolver iocResolver, Action<TDbContext> contextAction)
            where TDbContext : DbContext
        {
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    var context = uowManager.Object.Current.GetDbContext<TDbContext>(MultiTenancySides.Host);

                    contextAction(context);

                    uow.Complete();
                }
            }
        }
    }
}
