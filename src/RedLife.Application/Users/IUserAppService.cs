using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RedLife.Roles.Dto;
using RedLife.Users.Dto;

namespace RedLife.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task ChangeLanguage(ChangeUserLanguageDto input);

        Task<bool> ChangePassword(ChangePasswordDto input);

        public ListResultDto<UserDto> GetTransfusionCenters();

        public ListResultDto<UserDto> GetDonors();
    }
}
