using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RedLife.Authorization;
using RedLife.Authorization.Roles;
using RedLife.Authorization.Users;
using System.Linq;

namespace RedLife.EntityFrameworkCore.Seed.Tenants
{
    public class TenantRoleAndUserBuilder
    {
        private readonly RedLifeDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(RedLifeDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateRolesAndUsers();
        }


        private void CreateRolesAndUsers()
        {
            CreateAdminRoleAndUser();
            CreateCenterAdminRoleAndUser();
            CreateHospitalAdminRoleAndUser();

            CreateCenterPersonnelRoleAndUser();
            CreateHospitalPersonnelRoleAndUser();

            CreateDonorRoleAndUser();
        }

        private void CreateAdminRoleAndUser()
        {
            var adminRole = CreateAdminRole();
            CreateAdminUser(adminRole);
        }


        private void CreateCenterAdminRoleAndUser()
        {
            var centerAdminRole = CreateCenterAdminRole();
            CreateCenterAdminUser(centerAdminRole);
        }

        private void CreateCenterPersonnelRoleAndUser()
        {
            var centerPersonnelRole = CreateCenterPersonnelRole();
            CreateCenterPersonnelUser(centerPersonnelRole);
        }

        private void CreateHospitalAdminRoleAndUser()
        {
            var hospitalAdminRole = CreateHospitalAdminRole();
            CreateHospitalAdminUser(hospitalAdminRole);
        }

        private void CreateHospitalPersonnelRoleAndUser()
        {
            var hospitalPersonnelRole = CreateHospitalPersonnelRole();
            CreateHospitalPersonnelUser(hospitalPersonnelRole);
        }

        private void CreateDonorRoleAndUser()
        {
            var donorRole = CreateDonorRole();
            CreateDonorUser(donorRole);
        }

        public long GetAndUpdateLastUserId()
        {
            var lastUserId = _context.LastUserId.FirstOrDefault();
            lastUserId.Counter++;
            _context.LastUserId.Update(lastUserId);
            return lastUserId.Counter;
        }

        private Role CreateAdminRole()
        {
            var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            // Grant all permissions to admin role

            var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == adminRole.Id)
                .Select(p => p.Name)
                .ToList();

            var permissions = PermissionFinder
                .GetAllPermissions(new RedLifeAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                            !grantedPermissions.Contains(p.Name) &&
                            p.Name == PermissionNames.Admin                 ||
                            p.Name == PermissionNames.Pages_Users           ||
                            p.Name == PermissionNames.Pages_Tenants         ||
                            p.Name == PermissionNames.Pages_Roles           ||

                            p.Name == PermissionNames.Appointments_Get      ||
                            p.Name == PermissionNames.Appointments_Create   ||
                            p.Name == PermissionNames.Appointments_Update   ||
                            p.Name == PermissionNames.Appointments_Delete   ||

                            p.Name == PermissionNames.Users_GetCenters      ||
                            p.Name == PermissionNames.Users_GetDonors       ||

                            p.Name == PermissionNames.Donations_Get         ||
                            p.Name == PermissionNames.Donations_Create      ||
                            p.Name == PermissionNames.Donations_Update      ||
                            p.Name == PermissionNames.Donations_Delete
                            )
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = adminRole.Id
                    })
                );
                _context.SaveChanges();
            }

            return adminRole;
        }
        
        private Role CreateCenterAdminRole()
        {
            var centerAdminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.CenterAdmin);
            if (centerAdminRole == null)
            {
                centerAdminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.CenterAdmin, StaticRoleNames.Tenants.CenterAdmin) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            // Grant all permissions to admin role

            var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == centerAdminRole.Id)
                .Select(p => p.Name)
                .ToList();

            var permissions = PermissionFinder
                .GetAllPermissions(new RedLifeAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                            !grantedPermissions.Contains(p.Name) &&
                            p.Name == PermissionNames.CenterAdmin ||
                            p.Name == PermissionNames.Appointments_Get ||
                            p.Name == PermissionNames.Appointments_Update ||
                            p.Name == PermissionNames.Appointments_Delete ||

                            p.Name == PermissionNames.Donations_Get ||
                            p.Name == PermissionNames.Donations_Create ||
                            p.Name == PermissionNames.Donations_Update ||
                            p.Name == PermissionNames.Donations_Delete ||

                            p.Name == PermissionNames.Users_GetDonors
                            )
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = centerAdminRole.Id
                    })
                );
                _context.SaveChanges();
            }

            return centerAdminRole;
        }
        
        private Role CreateHospitalAdminRole()
        {
            var hospitalAdminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.HospitalAdmin);
            if (hospitalAdminRole == null)
            {
                hospitalAdminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.HospitalAdmin, StaticRoleNames.Tenants.HospitalAdmin) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            // Grant all permissions to admin role

            var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == hospitalAdminRole.Id)
                .Select(p => p.Name)
                .ToList();

            var permissions = PermissionFinder
                .GetAllPermissions(new RedLifeAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                            !grantedPermissions.Contains(p.Name) &&
                            p.Name == PermissionNames.HospitalAdmin
                            )
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = hospitalAdminRole.Id
                    })
                );
                _context.SaveChanges();
            }

            return hospitalAdminRole;
        }
        
        private Role CreateHospitalPersonnelRole()
        {
            var hospitalPersonnelRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.HospitalPersonnel);
            if (hospitalPersonnelRole == null)
            {
                hospitalPersonnelRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.HospitalPersonnel, StaticRoleNames.Tenants.HospitalPersonnel) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            // Grant all permissions to personnel role

            var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == hospitalPersonnelRole.Id)
                .Select(p => p.Name)
                .ToList();

            var permissions = PermissionFinder
                .GetAllPermissions(new RedLifeAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                            !grantedPermissions.Contains(p.Name) &&
                            p.Name == PermissionNames.HospitalPersonnel)
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = hospitalPersonnelRole.Id
                    })
                );
                _context.SaveChanges();
            }

            return hospitalPersonnelRole;
        }
        
        private Role CreateCenterPersonnelRole()
        {
            var centerPersonnelRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.CenterPersonnel);
            if (centerPersonnelRole == null)
            {
                centerPersonnelRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.CenterPersonnel, StaticRoleNames.Tenants.CenterPersonnel) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            // Grant all permissions to personnel role

            var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == centerPersonnelRole.Id)
                .Select(p => p.Name)
                .ToList();

            var permissions = PermissionFinder
                .GetAllPermissions(new RedLifeAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                            !grantedPermissions.Contains(p.Name) &&
                            p.Name == PermissionNames.CenterPersonnel ||
                            p.Name == PermissionNames.Appointments_Get ||

                            p.Name == PermissionNames.Donations_Get ||
                            p.Name == PermissionNames.Donations_Create ||
                            p.Name == PermissionNames.Donations_Update ||
                            p.Name == PermissionNames.Donations_Delete)
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = centerPersonnelRole.Id
                    })
                );
                _context.SaveChanges();
            }

            return centerPersonnelRole;
        }

        private Role CreateDonorRole()
        {
            var donorRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Donor);
            if (donorRole == null)
            {
                donorRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Donor, StaticRoleNames.Tenants.Donor) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            // Grant all permissions to donor role

            var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == donorRole.Id)
                .Select(p => p.Name)
                .ToList();

            var permissions = PermissionFinder
                .GetAllPermissions(new RedLifeAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                            !grantedPermissions.Contains(p.Name) &&
                            // write permission names here with || between them
                            p.Name == PermissionNames.Donor ||
                            p.Name == PermissionNames.Appointments_Get ||
                            p.Name == PermissionNames.Appointments_Create ||
                            p.Name == PermissionNames.Appointments_Update ||
                            p.Name == PermissionNames.Appointments_Delete ||

                            p.Name == PermissionNames.Users_GetCenters ||

                            p.Name == PermissionNames.Donations_Get)
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = donorRole.Id
                    })
                );
                _context.SaveChanges();
            }

            return donorRole;
        }

        private void CreateAdminUser(Role adminRole)
        {
            var adminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(_tenantId, "admin@defaulttenant.com");
                adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
                adminUser.IsEmailConfirmed = true;
                adminUser.IsActive = true;
                adminUser.Id = GetAndUpdateLastUserId();

                _context.Users.Add(adminUser);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
                _context.SaveChanges();
            }
        }

        private void CreateCenterAdminUser(Role centerAdminRole)
        {
            var centerAdminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == User.CenterAdminUserName);
            if (centerAdminUser == null)
            {
                centerAdminUser = User.CreateTenantCenterAdminUser(_tenantId, "centerAdmin@defaulttenant.com");
                centerAdminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(centerAdminUser, "123qwe");
                centerAdminUser.IsEmailConfirmed = true;
                centerAdminUser.IsActive = true;
                centerAdminUser.Id = GetAndUpdateLastUserId();
                centerAdminUser.InstitutionName = "Regina Maria";
                centerAdminUser.EmployerId = centerAdminUser.Id;
                centerAdminUser.Country = "Romania";
                centerAdminUser.City = "Bucharest";
                centerAdminUser.County = "Bucharest";
                centerAdminUser.Number = "15";
                centerAdminUser.Street = "Brancusi";

                _context.Users.Add(centerAdminUser);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, centerAdminUser.Id, centerAdminRole.Id));
                _context.SaveChanges();

                // transfusion center 2
                var centerAdminUser2 = User.CreateTenantCenterAdminUser(_tenantId, "centerAdmin2@defaulttenant.com");
                centerAdminUser2.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(centerAdminUser2, "123qwe");
                centerAdminUser2.IsEmailConfirmed = true;
                centerAdminUser2.IsActive = true;
                centerAdminUser2.Id = GetAndUpdateLastUserId();
                centerAdminUser2.InstitutionName = "Sinevo";
                centerAdminUser2.EmployerId = centerAdminUser2.Id;
                centerAdminUser2.Country = "Romania";
                centerAdminUser2.City = "Bucharest";
                centerAdminUser2.County = "Bucharest";
                centerAdminUser2.Number = "7";
                centerAdminUser2.Street = "Veteranilor";

                _context.Users.Add(centerAdminUser2);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, centerAdminUser2.Id, centerAdminRole.Id));
                _context.SaveChanges();

            }
        }

        private void CreateHospitalAdminUser(Role hospitalAdminRole)
        {
            var hospitalAdminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == User.HospitalAdminUserName);
            if (hospitalAdminUser == null)
            {
                hospitalAdminUser = User.CreateTenantHospitalAdminUser(_tenantId, "hospitalAdmin@defaulttenant.com");
                hospitalAdminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(hospitalAdminUser, "123qwe");
                hospitalAdminUser.IsEmailConfirmed = true;
                hospitalAdminUser.IsActive = true;
                hospitalAdminUser.Id = GetAndUpdateLastUserId();

                _context.Users.Add(hospitalAdminUser);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, hospitalAdminUser.Id, hospitalAdminRole.Id));
                _context.SaveChanges();
            }
        }

        private void CreateHospitalPersonnelUser(Role hospitalPersonnelRole)
        {
            var hospitalPersonnelUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == User.HospitalPersonnelUserName);
            if (hospitalPersonnelUser == null)
            {
                hospitalPersonnelUser = User.CreateTenantHospitalPersonnelUser(_tenantId, "hospitalPersonnel@defaulttenant.com");
                hospitalPersonnelUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(hospitalPersonnelUser, "123qwe");
                hospitalPersonnelUser.IsEmailConfirmed = true;
                hospitalPersonnelUser.IsActive = true;
                hospitalPersonnelUser.Id = GetAndUpdateLastUserId();

                _context.Users.Add(hospitalPersonnelUser);
                _context.SaveChanges();

                // Assign Personnel role to personnel user
                _context.UserRoles.Add(new UserRole(_tenantId, hospitalPersonnelUser.Id, hospitalPersonnelRole.Id));
                _context.SaveChanges();
            }
        }

        private void CreateCenterPersonnelUser(Role centerPersonnelRole)
        {
            var centerPersonnelUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == User.CenterPersonnelUserName);
            if (centerPersonnelUser == null)
            {
                centerPersonnelUser = User.CreateTenantCenterPersonnelUser(_tenantId, "centerPersonnel@defaulttenant.com");
                centerPersonnelUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(centerPersonnelUser, "123qwe");
                centerPersonnelUser.IsEmailConfirmed = true;
                centerPersonnelUser.IsActive = true;
                centerPersonnelUser.Id = GetAndUpdateLastUserId();

                _context.Users.Add(centerPersonnelUser);
                _context.SaveChanges();

                // Assign Personnel role to personnel user
                _context.UserRoles.Add(new UserRole(_tenantId, centerPersonnelUser.Id, centerPersonnelRole.Id));
                _context.SaveChanges();
            }
        }
       
        private void CreateDonorUser(Role donorRole)
        {
            var donorUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == User.DonorUserName);
            if (donorUser == null)
            {
                var donorUser1 = User.CreateTenantDonorUser(_tenantId, "donor1@defaulttenant.com");
                donorUser1.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(donorUser1, "123qwe");
                donorUser1.Surname = "Istrate";
                donorUser1.Name = "Madalina";
                donorUser1.IsEmailConfirmed = true;
                donorUser1.IsActive = true;
                donorUser1.Id = 2990407460021;

                _context.Users.Add(donorUser1);
                _context.SaveChanges();

                // Assign Personnel role to personnel user
                _context.UserRoles.Add(new UserRole(_tenantId, donorUser1.Id, donorRole.Id));
                _context.SaveChanges();


                var donorUser2 = User.CreateTenantDonorUser(_tenantId, "donor2@defaulttenant.com");
                donorUser2.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(donorUser2, "123qwe");
                donorUser2.Surname = "Zota";
                donorUser2.Name = "Alexandru";
                donorUser2.IsEmailConfirmed = true;
                donorUser2.IsActive = true;
                donorUser2.Id = 6990407460021;

                _context.Users.Add(donorUser2);
                _context.SaveChanges();

                // Assign Personnel role to personnel user
                _context.UserRoles.Add(new UserRole(_tenantId, donorUser2.Id, donorRole.Id));
                _context.SaveChanges();
            }
        }
    }
}
