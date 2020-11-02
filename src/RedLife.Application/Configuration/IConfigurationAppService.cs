using System.Threading.Tasks;
using RedLife.Configuration.Dto;

namespace RedLife.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
