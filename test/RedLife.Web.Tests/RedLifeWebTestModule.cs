using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using RedLife.EntityFrameworkCore;
using RedLife.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace RedLife.Web.Tests
{
    [DependsOn(
        typeof(RedLifeWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class RedLifeWebTestModule : AbpModule
    {
        public RedLifeWebTestModule(RedLifeEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(RedLifeWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(RedLifeWebMvcModule).Assembly);
        }
    }
}