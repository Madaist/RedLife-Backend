using System.Collections.Generic;
using Abp.Configuration;
using Abp.Net.Mail;

namespace RedLife.Configuration
{
    public class AppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(AppSettingNames.UiTheme, "red", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true),
                
                new SettingDefinition(EmailSettingNames.DefaultFromAddress, "redlife.noreply@gmail.com", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User),
                new SettingDefinition(EmailSettingNames.DefaultFromDisplayName, "Red Life",  scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true),
                new SettingDefinition(EmailSettingNames.Smtp.Port, "587",  scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User),
                new SettingDefinition(EmailSettingNames.Smtp.UseDefaultCredentials, "false",  scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User),
                new SettingDefinition(EmailSettingNames.Smtp.EnableSsl, "true",  scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User),
                new SettingDefinition(EmailSettingNames.Smtp.Host, "smtp.gmail.com",  scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User),
                new SettingDefinition(EmailSettingNames.Smtp.UserName, "redlife.noreply",  scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User),
                new SettingDefinition(EmailSettingNames.Smtp.Password, "redlife2021!",  scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User),

            };
        }
    }
}
