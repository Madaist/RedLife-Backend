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

        public const string Donations_Get = "Donations.Get";
        public const string Donations_Create = "Donations.Create";
        public const string Donations_Update = "Donations.Update";
        public const string Donations_Delete = "Donations.Delete";
        public const string Donations_SeeDonor = "Donations.SeeDonor";
        public const string Donations_SeeCenter = "Donations.SeeCenter";
    }
}
