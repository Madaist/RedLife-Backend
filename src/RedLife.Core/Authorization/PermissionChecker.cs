using Abp.Authorization;
using RedLife.Authorization.Roles;
using RedLife.Authorization.Users;

namespace RedLife.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
