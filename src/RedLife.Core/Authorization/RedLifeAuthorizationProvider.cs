using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace RedLife.Authorization
{
    public class RedLifeAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));

            context.CreatePermission(PermissionNames.Appointments_Get, L("AppointmentsGet"));
            context.CreatePermission(PermissionNames.Appointments_Create, L("AppointmentsCreate"));
            context.CreatePermission(PermissionNames.Appointments_Update, L("AppointmentsUpdate"));
            context.CreatePermission(PermissionNames.Appointments_Delete, L("AppointmentsDelete"));
            context.CreatePermission(PermissionNames.Appointments_SeeDonor, L("AppointmentsSeeDonor"));
            context.CreatePermission(PermissionNames.Appointments_None, L("AppointmentsNone"));

            context.CreatePermission(PermissionNames.Users_GetCenters, L("UsersGetCenters"));
            context.CreatePermission(PermissionNames.Users_GetDonors, L("Users_GetDonors"));

            context.CreatePermission(PermissionNames.Donations_Get, L("DonationsGet"));
            context.CreatePermission(PermissionNames.Donations_Create, L("DonationsCreate"));
            context.CreatePermission(PermissionNames.Donations_Update, L("DonationsUpdate"));
            context.CreatePermission(PermissionNames.Donations_Delete, L("DonationsDelete"));
            context.CreatePermission(PermissionNames.Donations_SeeDonor, L("DonationsSeeDonor"));
            context.CreatePermission(PermissionNames.Donations_SeeCenter, L("DonationsSeeCenter"));

        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, RedLifeConsts.LocalizationSourceName);
        }
    }
}
