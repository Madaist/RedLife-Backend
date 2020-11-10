using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Authorization.Users;
using Abp.Extensions;
using RedLife.Core.Appointments;

namespace RedLife.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public string Country { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public DateTime BirthDate { get; set; }
        public string InstitutionName { get; set; }

        [InverseProperty("Center")]
        public virtual ICollection<Appointment> CenterAppointments { get; set; }

        [InverseProperty("Donor")]
        public virtual ICollection<Appointment> DonorAppointments { get; set; }

        public long EmployerId { get; set; }
        public virtual User Employer { get; set; }

        public const string DefaultPassword = "123qwe";

        public const string HospitalAdminUserName = "HospitalAdmin";
        public const string CenterAdminUserName = "CenterAdmin";

        public const string HospitalPersonnelUserName = "HospitalPersonnel";
        public const string CenterPersonnelUserName = "CenterPersonnel";

        public const string DonorUserName = "Donor";


        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }

        public static User CreateTenantCenterAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = CenterAdminUserName,
                Name = CenterAdminUserName,
                Surname = CenterAdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }

        public static User CreateTenantHospitalAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = HospitalAdminUserName,
                Name = HospitalAdminUserName,
                Surname = HospitalAdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }

        public static User CreateTenantCenterPersonnelUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = CenterPersonnelUserName,
                Name = CenterPersonnelUserName,
                Surname = CenterPersonnelUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }

        public static User CreateTenantHospitalPersonnelUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = HospitalPersonnelUserName,
                Name = HospitalPersonnelUserName,
                Surname = HospitalPersonnelUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }

        public static User CreateTenantDonorUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = DonorUserName,
                Name = DonorUserName,
                Surname = DonorUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }
    }
}
