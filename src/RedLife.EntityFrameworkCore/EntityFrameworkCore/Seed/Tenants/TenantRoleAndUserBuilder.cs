using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using RedLife.Authorization;
using RedLife.Authorization.Roles;
using RedLife.Authorization.Users;
using RedLife.Core.LastId;

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
            //CreateInitialUserId();
            CreateRolesAndUsers();
        }

        //private void CreateInitialUserId()
        //{
        //    var id = _context.LastUserId.IgnoreQueryFilters().FirstOrDefault();
        //    if (id == null)
        //    {
        //        _context.LastUserId.Add(new LastUserId() { LastId = 0 });
        //        _context.SaveChanges();
        //    }
        //}

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
            lastUserId.LastId++;
            _context.LastUserId.Update(lastUserId);
            return lastUserId.LastId;
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
                            !grantedPermissions.Contains(p.Name))
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
                            !grantedPermissions.Contains(p.Name))
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

                _context.Users.Add(centerAdminUser);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, centerAdminUser.Id, centerAdminRole.Id));
                _context.SaveChanges();
            }
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
                            !grantedPermissions.Contains(p.Name))
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
                            !grantedPermissions.Contains(p.Name))
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
                            !grantedPermissions.Contains(p.Name))
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

        private Role CreateDonorRole()
        {
            var donorRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Donor);
            if (donorRole == null)
            {
                donorRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Donor, StaticRoleNames.Tenants.Donor) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            // Grant all permissions to personnel role

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
                            p.Name == PermissionNames.Appointment_Create
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
                        RoleId = donorRole.Id
                    })
                );
                _context.SaveChanges();
            }

            return donorRole;
        }

        private void CreateDonorUser(Role donorRole)
        {
            var donorUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == User.DonorUserName);
            if (donorUser == null)
            {
                donorUser = User.CreateTenantDonorUser(_tenantId, "donor@defaulttenant.com");
                donorUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(donorUser, "123qwe");
                donorUser.IsEmailConfirmed = true;
                donorUser.IsActive = true;
                donorUser.Id = 2990407460021;

                _context.Users.Add(donorUser);
                _context.SaveChanges();

                // Assign Personnel role to personnel user
                _context.UserRoles.Add(new UserRole(_tenantId, donorUser.Id, donorRole.Id));
                _context.SaveChanges();
            }
        }
    }
}
