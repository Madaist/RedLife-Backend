using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using RedLife.Configuration.Dto;

namespace RedLife.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : RedLifeAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
