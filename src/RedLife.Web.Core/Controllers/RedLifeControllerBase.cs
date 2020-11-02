using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace RedLife.Controllers
{
    public abstract class RedLifeControllerBase: AbpController
    {
        protected RedLifeControllerBase()
        {
            LocalizationSourceName = RedLifeConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
