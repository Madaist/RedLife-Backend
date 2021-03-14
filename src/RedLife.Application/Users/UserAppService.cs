using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using RedLife.Authorization;
using RedLife.Authorization.Accounts;
using RedLife.Authorization.Roles;
using RedLife.Authorization.Users;
using RedLife.Roles.Dto;
using RedLife.Users.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RedLife.Core.LastId;
using static RedLife.Authorization.Roles.StaticRoleNames;
using System;

namespace RedLife.Users
{
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;
        private readonly LastUserIdManager _lastUserIdManager;

        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager,
            LastUserIdManager lastUserIdManager)
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
            _lastUserIdManager = lastUserIdManager;
        }

        [AbpAuthorize(PermissionNames.Pages_Users)]
        public override async Task<PagedResultDto<UserDto>> GetAllAsync(PagedUserResultRequestDto input)
        {
            var currentUser = _userManager.GetUserById(_abpSession.UserId ?? 0);
            var userRole = _userManager.GetRolesAsync(currentUser).Result.FirstOrDefault();

            List<UserDto> userDtoOutput = new List<UserDto>();

            if (userRole == Tenants.Admin)
            {
                return await base.GetAllAsync(input);
            }
            else if (userRole == Tenants.CenterAdmin)
            {
                userDtoOutput = GetCenterPersonnelUsers(currentUser.Id);

            }
            else if (userRole == Tenants.HospitalAdmin)
            {
                userDtoOutput = GetHospitalPersonnelUsers(currentUser.Id);
            };

            return new PagedResultDto<UserDto>
            {
                Items = userDtoOutput,
                TotalCount = userDtoOutput.Count
            };

        }

        [AbpAuthorize(PermissionNames.Pages_Users)]
        public override async Task<UserDto> CreateAsync(CreateUserDto input)
        {
            CheckCreatePermission();

            var user = ObjectMapper.Map<User>(input);
            user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;

            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            if (input.RoleNames != null)
            {
                if (input.RoleNames.Contains(Tenants.Donor))
                {
                    user.Id = (long)input.SocialSecurityNumber;
                }
                else
                {
                    user.Id = _lastUserIdManager.GetAndUpdateLastUserId();
                }
            }

            CheckErrors(await _userManager.CreateAsync(user, input.Password));
            CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));

            CurrentUnitOfWork.SaveChanges();
            return MapToEntityDto(user);
        }

        [AbpAuthorize(PermissionNames.Pages_Users)]
        public override async Task<UserDto> UpdateAsync(UserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);
            //if we don't have this, for users that don't have an employer, employerId will come as 0
            //as we don't have any userId equal to 0, we will get an error from the database
            if(input.EmployerId == 0)
            {
                input.EmployerId = 1;
            }

            MapToEntity(input, user);

            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            return await GetAsync(input);
        }


        [AbpAuthorize(PermissionNames.Pages_Users)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }


        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        [AbpAuthorize(PermissionNames.Users_GetById)]
        public override Task<UserDto> GetAsync(EntityDto<long> input)
        {
            return base.GetAsync(input);
        }

        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var roleIds = user.Roles.Select(x => x.RoleId).ToArray();

            var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);

            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();

            return userDto;
        }

        protected override IQueryable<User> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }


        [AbpAuthorize(PermissionNames.Users_GetById)]
        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }


        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }


        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }


        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attemping to change password.");
            }
            long userId = _abpSession.UserId.Value;
            var user = await _userManager.GetUserByIdAsync(userId);
            var loginAsync = await _logInManager.LoginAsync(user.UserName, input.CurrentPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Existing Password' did not match the one on record.  Please try again or contact an administrator for assistance in resetting your password.");
            }
            if (!new Regex(AccountAppService.PasswordRegex).IsMatch(input.NewPassword))
            {
                throw new UserFriendlyException("Passwords must be at least 8 characters, contain a lowercase, uppercase, and number.");
            }
            user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
            CurrentUnitOfWork.SaveChanges();
            return true;
        }


        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attemping to reset password.");
            }
            long currentUserId = _abpSession.UserId.Value;
            var currentUser = await _userManager.GetUserByIdAsync(currentUserId);
            var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Admin Password' did not match the one on record.  Please try again.");
            }
            if (currentUser.IsDeleted || !currentUser.IsActive)
            {
                return false;
            }
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains(Tenants.Admin) && !roles.Contains(Tenants.HospitalAdmin) && !roles.Contains(Tenants.CenterAdmin))
            {
                throw new UserFriendlyException("Only administrators may reset passwords.");
            }

            var user = await _userManager.GetUserByIdAsync(input.UserId);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                CurrentUnitOfWork.SaveChanges();
            }

            return true;
        }


        [AbpAuthorize(PermissionNames.Users_GetCenters)]
        public ListResultDto<UserDto> GetTransfusionCenters()
        {
            var transfusionCenters = _userManager.GetUsersInRoleAsync(Tenants.CenterAdmin).Result;
            return new ListResultDto<UserDto>(ObjectMapper.Map<List<UserDto>>(transfusionCenters));
        }


        [AbpAuthorize(PermissionNames.Users_GetDonors)]
        public ListResultDto<UserDto> GetDonors()
        {
            var donors = _userManager.GetUsersInRoleAsync(Tenants.Donor).Result;
            return new ListResultDto<UserDto>(ObjectMapper.Map<List<UserDto>>(donors));
        }

        [AbpAuthorize(PermissionNames.Users_GetHospitals)]
        public ListResultDto<UserDto> GetHospitals()
        {
            var hospitals = _userManager.GetUsersInRoleAsync(Tenants.HospitalAdmin).Result;
            return new ListResultDto<UserDto>(ObjectMapper.Map<List<UserDto>>(hospitals));
        }

        private List<UserDto> GetCenterPersonnelUsers(long centerAdminId)
        {
            var centerPersonnelUsers = _userManager
                    .GetUsersInRoleAsync(Tenants.CenterPersonnel)
                    .Result
                    .Where(x => x.EmployerId == centerAdminId)
                    .ToList();
            return new List<UserDto>(ObjectMapper.Map<List<UserDto>>(centerPersonnelUsers));
        }

        private List<UserDto> GetHospitalPersonnelUsers(long hospitalAdminId)
        {
            var hospitalPersonnelUsers = _userManager
                    .GetUsersInRoleAsync(Tenants.HospitalPersonnel)
                    .Result
                    .Where(x => x.EmployerId == hospitalAdminId)
                    .ToList();
            return new List<UserDto>(ObjectMapper.Map<List<UserDto>>(hospitalPersonnelUsers));
        }

    }
}

