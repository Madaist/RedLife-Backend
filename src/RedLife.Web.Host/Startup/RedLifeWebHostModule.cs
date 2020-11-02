using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using RedLife.Configuration;

namespace RedLife.Web.Host.Startup
{
    [DependsOn(
       typeof(RedLifeWebCoreModule))]
    public class RedLifeWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public RedLifeWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(RedLifeWebHostModule).GetAssembly());
        }
    }
}
