using System.Runtime.Serialization;

namespace RedLife.Authorization.Roles
{
    public static class StaticRoleNames
    {
        public static class Host
        {
            public const string Admin = "Admin";
        }

        public static class Tenants
        {
            public const string Admin = "Admin";
            public const string CenterAdmin = "CenterAdmin";
            public const string HospitalAdmin = "HospitalAdmin";

            public const string Donor = "Donor";
            public const string CenterPersonnel = "CenterPersonnel";
            public const string HospitalPersonnel = "HospitalPersonnel";
        }
    }
}
