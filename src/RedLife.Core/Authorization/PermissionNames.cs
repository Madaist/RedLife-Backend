namespace RedLife.Authorization
{
    public static class PermissionNames
    {
        public const string Pages_Tenants = "Pages.Tenants";
        public const string Pages_Users = "Pages.Users";
        public const string Pages_Roles = "Pages.Roles";

        public const string Appointments_Get = "Appointments.Get";
        public const string Appointments_Create = "Appointments.Create";
        public const string Appointments_Update = "Appointments.Update";
        public const string Appointments_Delete = "Appointments.Delete";
        public const string Appointments_SeeDonor = "Appointments.SeeDonor";
        public const string Appointments_None = "Appointments.None"; //to be deleted

        public const string Users_GetCenters = "Users.GetCenters";
        public const string Users_GetDonors = "Users.GetDonors";
    }
}
