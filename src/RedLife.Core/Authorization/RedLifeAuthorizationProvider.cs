using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace RedLife.Authorization
{
    public class RedLifeAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Admin, L("Admin"));
            context.CreatePermission(PermissionNames.Donor, L("Donor"));
            context.CreatePermission(PermissionNames.CenterAdmin, L("Center Admin"));
            context.CreatePermission(PermissionNames.HospitalAdmin, L("Hospital Admin"));
            context.CreatePermission(PermissionNames.CenterPersonnel, L("Center Personnel"));
            context.CreatePermission(PermissionNames.HospitalPersonnel, L("Hospital Personnel"));

            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));

            context.CreatePermission(PermissionNames.Appointments_Get, L("Appointments Get"));
            context.CreatePermission(PermissionNames.Appointments_Create, L("Appointments Create"));
            context.CreatePermission(PermissionNames.Appointments_Update, L("Appointments Update"));
            context.CreatePermission(PermissionNames.Appointments_Delete, L("Appointments Delete"));

            context.CreatePermission(PermissionNames.Users_GetCenters, L("Get Centers"));
            context.CreatePermission(PermissionNames.Users_GetDonors, L("Get Donors"));
            context.CreatePermission(PermissionNames.Users_GetHospitals, L("Get Hospitals"));
            context.CreatePermission(PermissionNames.Users_GetById, L("Get User By Id"));

            context.CreatePermission(PermissionNames.Donations_Get, L("Donations Get"));
            context.CreatePermission(PermissionNames.Donations_Create, L("Donations Create"));
            context.CreatePermission(PermissionNames.Donations_Update, L("Donations Update"));
            context.CreatePermission(PermissionNames.Donations_Delete, L("Donations Delete"));

            context.CreatePermission(PermissionNames.Transfusions_Get, L("Transfusions Get"));
            context.CreatePermission(PermissionNames.Transfusions_Create, L("Transfusions Create"));
            context.CreatePermission(PermissionNames.Transfusions_Update, L("Transfusions Update"));
            context.CreatePermission(PermissionNames.Transfusions_Delete, L("Transfusions Delete"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, RedLifeConsts.LocalizationSourceName);
        }
    }
}
