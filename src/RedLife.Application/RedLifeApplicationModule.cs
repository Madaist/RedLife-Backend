using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using RedLife.Authorization;

namespace RedLife
{
    [DependsOn(
        typeof(RedLifeCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class RedLifeApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<RedLifeAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(RedLifeApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
