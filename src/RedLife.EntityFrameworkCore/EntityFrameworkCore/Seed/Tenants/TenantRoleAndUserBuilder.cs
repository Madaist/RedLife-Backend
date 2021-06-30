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
using RedLife.Core.Donations;
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
                            p.Name == PermissionNames.Users_GetHospitals    ||
                            p.Name == PermissionNames.Users_GetById         ||

                            p.Name == PermissionNames.Donations_Get         ||
                            p.Name == PermissionNames.Donations_Create      ||
                            p.Name == PermissionNames.Donations_Update      ||
                            p.Name == PermissionNames.Donations_Delete      ||

                            p.Name == PermissionNames.Transfusions_Get      ||
                            p.Name == PermissionNames.Transfusions_Create   ||
                            p.Name == PermissionNames.Transfusions_Update   ||
                            p.Name == PermissionNames.Transfusions_Delete
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

                            p.Name == PermissionNames.Users_GetDonors ||
                            p.Name == PermissionNames.Users_GetById ||
                            p.Name == PermissionNames.Pages_Users
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
                            p.Name == PermissionNames.HospitalAdmin ||
                            p.Name == PermissionNames.Users_GetById ||
                            p.Name == PermissionNames.Pages_Users ||

                            p.Name == PermissionNames.Transfusions_Get ||
                            p.Name == PermissionNames.Transfusions_Create ||
                            p.Name == PermissionNames.Transfusions_Update ||
                            p.Name == PermissionNames.Transfusions_Delete
                          
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
                            p.Name == PermissionNames.HospitalPersonnel ||
                            p.Name == PermissionNames.Users_GetById ||

                            p.Name == PermissionNames.Transfusions_Get ||
                            p.Name == PermissionNames.Transfusions_Create ||
                            p.Name == PermissionNames.Transfusions_Update ||
                            p.Name == PermissionNames.Transfusions_Delete)
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
                            p.Name == PermissionNames.Appointments_Create ||

                            p.Name == PermissionNames.Donations_Get ||
                            p.Name == PermissionNames.Donations_Create ||
                            p.Name == PermissionNames.Donations_Update ||
                            p.Name == PermissionNames.Donations_Delete ||
                            
                            p.Name == PermissionNames.Users_GetDonors ||
                            p.Name == PermissionNames.Users_GetById)
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
                            p.Name == PermissionNames.Donations_Get ||
                            p.Name == PermissionNames.Transfusions_Get
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

        private void CreateAdminUser(Role adminRole)
        {
            var adminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
            var leagueId = _context.Leagues.FirstOrDefault(league => league.Name == "Bronze").Id;

            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(_tenantId, "admin@defaulttenant.com");
                adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
                adminUser.Id = GetAndUpdateLastUserId();
                adminUser.Surname = "Admin";
                adminUser.Name = "Red Life";
                adminUser.LeagueId = leagueId;

                _context.Users.Add(adminUser);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
                _context.SaveChanges();
            }
        }

        private void CreateCenterAdminUser(Role centerAdminRole)
        {
            var centerAdminReginaMaria = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == User.CenterAdminUserName);
            var leagueId = _context.Leagues.FirstOrDefault(league => league.Name == "Bronze").Id;

            if (centerAdminReginaMaria == null)
            {
                centerAdminReginaMaria = User.CreateTenantCenterAdminUser(_tenantId, "admin@reginamaria.com");
                centerAdminReginaMaria.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(centerAdminReginaMaria, "123qwe");
                centerAdminReginaMaria.Id = GetAndUpdateLastUserId();
                centerAdminReginaMaria.InstitutionName = "Regina Maria";
                centerAdminReginaMaria.EmployerId = centerAdminReginaMaria.Id;
                centerAdminReginaMaria.Country = "Romania";
                centerAdminReginaMaria.City = "Bucharest";
                centerAdminReginaMaria.County = "Bucharest";
                centerAdminReginaMaria.Number = "15";
                centerAdminReginaMaria.Street = "Brancusi";
                centerAdminReginaMaria.UserName = "adminReginaMaria";
                centerAdminReginaMaria.CreationTime = new System.DateTime(2021, 1, 12);
                centerAdminReginaMaria.Surname = "Maria";
                centerAdminReginaMaria.Name = "Regina";
                centerAdminReginaMaria.LeagueId = leagueId;


                _context.Users.Add(centerAdminReginaMaria);
                _context.SaveChanges();

                // Assign Center Admin role to center admin user
                _context.UserRoles.Add(new UserRole(_tenantId, centerAdminReginaMaria.Id, centerAdminRole.Id));
                _context.SaveChanges();

                // transfusion center 2
                var centerAdminSinevo = User.CreateTenantCenterAdminUser(_tenantId, "admin@sinevo.com");
                centerAdminSinevo.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(centerAdminSinevo, "123qwe");
                centerAdminSinevo.Id = GetAndUpdateLastUserId();
                centerAdminSinevo.InstitutionName = "Sinevo";
                centerAdminSinevo.EmployerId = centerAdminSinevo.Id;
                centerAdminSinevo.Country = "Romania";
                centerAdminSinevo.City = "Bucharest";
                centerAdminSinevo.County = "Bucharest";
                centerAdminSinevo.Number = "7";
                centerAdminSinevo.Street = "Veteranilor";
                centerAdminSinevo.UserName = "adminSinevo";
                centerAdminSinevo.CreationTime = new System.DateTime(2021, 1, 12);
                centerAdminSinevo.Surname = "Center";
                centerAdminSinevo.Name = "Sinevo";
                centerAdminSinevo.LeagueId = leagueId;

                _context.Users.Add(centerAdminSinevo);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, centerAdminSinevo.Id, centerAdminRole.Id));
                _context.SaveChanges();

                // transfusion center 3
                var centerAdminCTS = User.CreateTenantCenterAdminUser(_tenantId, "admin@cts.com");
                centerAdminCTS.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(centerAdminCTS, "123qwe");
                centerAdminCTS.Id = GetAndUpdateLastUserId();
                centerAdminCTS.InstitutionName = "CTS";
                centerAdminCTS.EmployerId = centerAdminCTS.Id;
                centerAdminCTS.Country = "Romania";
                centerAdminCTS.City = "Bucharest";
                centerAdminCTS.County = "Bucharest";
                centerAdminCTS.Number = "2";
                centerAdminCTS.Street = "Doctor Constantin Caracaș";
                centerAdminCTS.UserName = "adminCTS";
                centerAdminCTS.CreationTime = new System.DateTime(2021, 2, 5);
                centerAdminCTS.Surname = "Center";
                centerAdminCTS.Name = "Transfusion";
                centerAdminCTS.LeagueId = leagueId;

                _context.Users.Add(centerAdminCTS);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, centerAdminCTS.Id, centerAdminRole.Id));
                _context.SaveChanges();

                // transfusion center 4
                var centerAdminCTSBacau = User.CreateTenantCenterAdminUser(_tenantId, "admin@ctsbacau.com");
                centerAdminCTSBacau.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(centerAdminCTSBacau, "123qwe");
                centerAdminCTSBacau.Id = GetAndUpdateLastUserId();
                centerAdminCTSBacau.InstitutionName = "CTS Bacau";
                centerAdminCTSBacau.EmployerId = centerAdminCTSBacau.Id;
                centerAdminCTSBacau.Country = "Romania";
                centerAdminCTSBacau.City = "Bacau";
                centerAdminCTSBacau.County = "Bacau";
                centerAdminCTSBacau.Number = "22";
                centerAdminCTSBacau.Street = "Marasesti";
                centerAdminCTSBacau.UserName = "adminCTSBacau";
                centerAdminCTSBacau.CreationTime = new System.DateTime(2021, 3, 20);
                centerAdminCTSBacau.Surname = "Center";
                centerAdminCTSBacau.Name = "Transfusion";
                centerAdminCTSBacau.LeagueId = leagueId;

                _context.Users.Add(centerAdminCTSBacau);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, centerAdminCTSBacau.Id, centerAdminRole.Id));
                _context.SaveChanges();


                // transfusion center 5
                var centerAdminCTSTimisoara = User.CreateTenantCenterAdminUser(_tenantId, "admin@ctstimisoara.com");
                centerAdminCTSTimisoara.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(centerAdminCTSTimisoara, "123qwe");
                centerAdminCTSTimisoara.Id = GetAndUpdateLastUserId();
                centerAdminCTSTimisoara.InstitutionName = "CTS Timisoara";
                centerAdminCTSTimisoara.EmployerId = centerAdminCTSTimisoara.Id;
                centerAdminCTSTimisoara.Country = "Romania";
                centerAdminCTSTimisoara.City = "Timisoara";
                centerAdminCTSTimisoara.County = "Timis";
                centerAdminCTSTimisoara.Number = "1";
                centerAdminCTSTimisoara.Street = "Martir Marius Ciopec";
                centerAdminCTSTimisoara.CreationTime = new System.DateTime(2021, 3, 20);
                centerAdminCTSTimisoara.Surname = "Center";
                centerAdminCTSTimisoara.Name = "Transfusion";
                centerAdminCTSTimisoara.LeagueId = leagueId;

                _context.Users.Add(centerAdminCTSTimisoara);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, centerAdminCTSTimisoara.Id, centerAdminRole.Id));
                _context.SaveChanges();

                // transfusion center 6
                var centerAdminCTSCluj = User.CreateTenantCenterAdminUser(_tenantId, "admin@ctscluj.com");
                centerAdminCTSCluj.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(centerAdminCTSCluj, "123qwe");
                centerAdminCTSCluj.Id = GetAndUpdateLastUserId();
                centerAdminCTSCluj.InstitutionName = "CTS Cluj";
                centerAdminCTSCluj.EmployerId = centerAdminCTSCluj.Id;
                centerAdminCTSCluj.Country = "Romania";
                centerAdminCTSCluj.City = "Cluj-Napoca";
                centerAdminCTSCluj.County = "Cluj";
                centerAdminCTSCluj.Number = "18";
                centerAdminCTSCluj.Street = "N. Balcescu";
                centerAdminCTSCluj.UserName = "adminCTSCluj";
                centerAdminCTSCluj.CreationTime = new System.DateTime(2021, 5, 20);
                centerAdminCTSCluj.Surname = "Center";
                centerAdminCTSCluj.Name = "Transfusion";
                centerAdminCTSCluj.LeagueId = leagueId;

                _context.Users.Add(centerAdminCTSCluj);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, centerAdminCTSCluj.Id, centerAdminRole.Id));
                _context.SaveChanges();

                // transfusion center 7
                var centerAdminCTSOradea = User.CreateTenantCenterAdminUser(_tenantId, "admin@ctsoradea.com");
                centerAdminCTSOradea.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(centerAdminCTSOradea, "123qwe");
                centerAdminCTSOradea.Id = GetAndUpdateLastUserId();
                centerAdminCTSOradea.InstitutionName = "CTS Oradea";
                centerAdminCTSOradea.EmployerId = centerAdminCTSOradea.Id;
                centerAdminCTSOradea.Country = "Romania";
                centerAdminCTSOradea.City = "Oradea";
                centerAdminCTSOradea.County = "Bihor";
                centerAdminCTSOradea.Number = "30";
                centerAdminCTSOradea.Street = "Louis Pasteur";
                centerAdminCTSOradea.UserName = "adminCTSOradea";
                centerAdminCTSOradea.CreationTime = new System.DateTime(2021, 6, 20);
                centerAdminCTSOradea.Surname = "Center";
                centerAdminCTSOradea.Name = "Transfusion";
                centerAdminCTSOradea.LeagueId = leagueId;

                _context.Users.Add(centerAdminCTSOradea);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, centerAdminCTSOradea.Id, centerAdminRole.Id));
                _context.SaveChanges();

                // transfusion center 8
                var centerAdminCTSArad = User.CreateTenantCenterAdminUser(_tenantId, "admin@ctsarad.com");
                centerAdminCTSArad.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(centerAdminCTSArad, "123qwe");
                centerAdminCTSArad.Id = GetAndUpdateLastUserId();
                centerAdminCTSArad.InstitutionName = "CTS Arad";
                centerAdminCTSArad.EmployerId = centerAdminCTSArad.Id;
                centerAdminCTSArad.Country = "Romania";
                centerAdminCTSArad.City = "Arad";
                centerAdminCTSArad.County = "Arad";
                centerAdminCTSArad.Number = "4";
                centerAdminCTSArad.Street = "Andrenyi Karoly";
                centerAdminCTSArad.UserName = "adminCTSArad";
                centerAdminCTSArad.CreationTime = new System.DateTime(2021, 6, 15);
                centerAdminCTSArad.Surname = "Center";
                centerAdminCTSArad.Name = "Transfusion";
                centerAdminCTSArad.LeagueId = leagueId;

                _context.Users.Add(centerAdminCTSArad);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, centerAdminCTSArad.Id, centerAdminRole.Id));
                _context.SaveChanges();

            }
        }

        private void CreateHospitalAdminUser(Role hospitalAdminRole)
        {
            var adminVBabes = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == User.HospitalAdminUserName);
            var leagueId = _context.Leagues.FirstOrDefault(league => league.Name == "Bronze").Id;

            if (adminVBabes == null)
            {
                adminVBabes = User.CreateTenantHospitalAdminUser(_tenantId, "admin@victorbabes.com");
                adminVBabes.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminVBabes, "123qwe");
                adminVBabes.Id = GetAndUpdateLastUserId();
                adminVBabes.InstitutionName = "Victor Babes";
                adminVBabes.EmployerId = adminVBabes.Id;
                adminVBabes.Country = "Romania";
                adminVBabes.City = "Bucharest";
                adminVBabes.County = "Bucharest";
                adminVBabes.Number = "281";
                adminVBabes.Street = "Mihai Bravu";
                adminVBabes.UserName = "adminBabes";
                adminVBabes.CreationTime = new System.DateTime(2021, 1, 15);
                adminVBabes.Surname = "Babes";
                adminVBabes.Name = "Victor";
                adminVBabes.LeagueId = leagueId;

                _context.Users.Add(adminVBabes);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, adminVBabes.Id, hospitalAdminRole.Id));
                _context.SaveChanges();


                var adminMilitar = User.CreateTenantHospitalAdminUser(_tenantId, "admin@spitalmilitar.com");
                adminMilitar.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminMilitar, "123qwe");
                adminMilitar.Id = GetAndUpdateLastUserId();
                adminMilitar.InstitutionName = "Military Hospital";
                adminMilitar.EmployerId = adminMilitar.Id;
                adminMilitar.Country = "Romania";
                adminMilitar.City = "Bucharest";
                adminMilitar.County = "Bucharest";
                adminMilitar.Number = "281";
                adminMilitar.Street = "Mihai Bravu";
                adminMilitar.CreationTime = new System.DateTime(2021, 2, 15);
                adminMilitar.Surname = "Hospital";
                adminMilitar.Name = "Military";
                adminMilitar.LeagueId = leagueId;

                _context.Users.Add(adminMilitar);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, adminMilitar.Id, hospitalAdminRole.Id));
                _context.SaveChanges();

                var adminInfectioaseCluj = User.CreateTenantHospitalAdminUser(_tenantId, "admin@boliinfectioasecluj.com");
                adminInfectioaseCluj.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminInfectioaseCluj, "123qwe");
                adminInfectioaseCluj.Id = GetAndUpdateLastUserId();
                adminInfectioaseCluj.InstitutionName = "Clinical Hospital for Infectious Diseases";
                adminInfectioaseCluj.EmployerId = adminInfectioaseCluj.Id;
                adminInfectioaseCluj.Country = "Romania";
                adminInfectioaseCluj.City = "Cluj";
                adminInfectioaseCluj.County = "Cluj-Napoca";
                adminInfectioaseCluj.Number = "23";
                adminInfectioaseCluj.Street = "Iuliu Moldovan";
                adminInfectioaseCluj.UserName = "adminBoliInfectioaseCluj";
                adminInfectioaseCluj.CreationTime = new System.DateTime(2021, 2, 15);
                adminInfectioaseCluj.Surname = "Hospital";
                adminInfectioaseCluj.Surname = "Cluj Infectious Diseases";
                adminInfectioaseCluj.LeagueId = leagueId;


                _context.Users.Add(adminInfectioaseCluj);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, adminInfectioaseCluj.Id, hospitalAdminRole.Id));
                _context.SaveChanges();

                var adminMavromati = User.CreateTenantHospitalAdminUser(_tenantId, "admin@mavromati.com");
                adminMavromati.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminMavromati, "123qwe");
                adminMavromati.Id = GetAndUpdateLastUserId();
                adminMavromati.InstitutionName = "Mavromati Emergency Hospital";
                adminMavromati.EmployerId = adminMavromati.Id;
                adminMavromati.Country = "Romania";
                adminMavromati.City = "Botosani";
                adminMavromati.County = "Botosani";
                adminMavromati.Number = "11";
                adminMavromati.Street = "Arhimandrit Marchian";
                adminMavromati.UserName = "adminMavromati";
                adminMavromati.CreationTime = new System.DateTime(2021, 3, 15);
                adminMavromati.Surname = "Hospital";
                adminMavromati.Name = "Mavromati";
                adminMavromati.LeagueId = leagueId;

                _context.Users.Add(adminMavromati);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, adminMavromati.Id, hospitalAdminRole.Id));
                _context.SaveChanges();

                var adminUrgentaBrasov = User.CreateTenantHospitalAdminUser(_tenantId, "admin@urgentabrasov.com");
                adminUrgentaBrasov.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUrgentaBrasov, "123qwe");
                adminUrgentaBrasov.Id = GetAndUpdateLastUserId();
                adminUrgentaBrasov.InstitutionName = "Emergency Clinical Hospital";
                adminUrgentaBrasov.EmployerId = adminUrgentaBrasov.Id;
                adminUrgentaBrasov.Country = "Romania";
                adminUrgentaBrasov.City = "Brasov";
                adminUrgentaBrasov.County = "Brasov";
                adminUrgentaBrasov.Number = "25";
                adminUrgentaBrasov.Street = "Calea București";
                adminUrgentaBrasov.UserName = "adminUrgentaBrasvov";
                adminUrgentaBrasov.CreationTime = new System.DateTime(2021, 3, 15);
                adminUrgentaBrasov.Surname = "Hospital";
                adminUrgentaBrasov.Name = "Emergency";
                adminUrgentaBrasov.LeagueId = leagueId;

                _context.Users.Add(adminUrgentaBrasov);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, adminUrgentaBrasov.Id, hospitalAdminRole.Id));
                _context.SaveChanges();

                var adminUrgentaCluj = User.CreateTenantHospitalAdminUser(_tenantId, "admin@urgentacluj.com");
                adminUrgentaCluj.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions()))
                    .HashPassword(adminUrgentaCluj, "123qwe");
                adminUrgentaCluj.Id = GetAndUpdateLastUserId();
                adminUrgentaCluj.InstitutionName = "Emergency Clinical Hospital";
                adminUrgentaCluj.EmployerId = adminUrgentaCluj.Id;
                adminUrgentaCluj.Country = "Romania";
                adminUrgentaCluj.City = "Cluj";
                adminUrgentaCluj.County = "Cluj Napoca";
                adminUrgentaCluj.Number = "3";
                adminUrgentaCluj.Street = "Clinicilor";
                adminUrgentaCluj.UserName = "adminUrgentaCluj";
                adminUrgentaCluj.CreationTime = new System.DateTime(2021, 4, 15);
                adminUrgentaCluj.Surname = "Hospital";
                adminUrgentaCluj.Name = "Emergency";
                adminUrgentaCluj.LeagueId = leagueId;

                _context.Users.Add(adminUrgentaCluj);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, adminUrgentaCluj.Id, hospitalAdminRole.Id));
                _context.SaveChanges();

                var adminUrgentaConstanta = User.CreateTenantHospitalAdminUser(_tenantId, "admin@urgentaconstanta.com");
                adminUrgentaConstanta.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions()))
                    .HashPassword(adminUrgentaConstanta, "123qwe");
                adminUrgentaConstanta.Id = GetAndUpdateLastUserId();
                adminUrgentaConstanta.InstitutionName = "County Emergency Hospital";
                adminUrgentaConstanta.EmployerId = adminUrgentaConstanta.Id;
                adminUrgentaConstanta.Country = "Romania";
                adminUrgentaConstanta.City = "Constanta";
                adminUrgentaConstanta.County = "Constanta";
                adminUrgentaConstanta.Number = "145";
                adminUrgentaConstanta.Street = "Tomis";
                adminUrgentaConstanta.UserName = "adminUrgentaConstanta";
                adminUrgentaConstanta.CreationTime = new System.DateTime(2021, 4, 15);
                adminUrgentaConstanta.Surname = "Hospital";
                adminUrgentaConstanta.Name = "Emergency";
                adminUrgentaConstanta.LeagueId = leagueId;

                _context.Users.Add(adminUrgentaConstanta);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, adminUrgentaConstanta.Id, hospitalAdminRole.Id));
                _context.SaveChanges();

                var adminColtea = User.CreateTenantHospitalAdminUser(_tenantId, "admin@coltea.com");
                adminColtea.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions()))
                    .HashPassword(adminColtea, "123qwe");
                adminColtea.Id = GetAndUpdateLastUserId();
                adminColtea.InstitutionName = "Colțea Clinical Hospital";
                adminColtea.EmployerId = adminColtea.Id;
                adminColtea.Country = "Romania";
                adminColtea.City = "Bucharest";
                adminColtea.County = "Bucharest";
                adminColtea.Number = "1";
                adminColtea.Street = "Ion C. Brătianu";
                adminColtea.UserName = "adminColtea";
                adminColtea.CreationTime = new System.DateTime(2021, 5, 15);
                adminColtea.Surname = "Hospital";
                adminColtea.Name = "Coltea";
                adminColtea.LeagueId = leagueId;

                _context.Users.Add(adminColtea);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, adminColtea.Id, hospitalAdminRole.Id));
                _context.SaveChanges();


                var adminElias = User.CreateTenantHospitalAdminUser(_tenantId, "admin@elias.com");
                adminElias.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions()))
                    .HashPassword(adminElias, "123qwe");
                adminElias.Id = GetAndUpdateLastUserId();
                adminElias.InstitutionName = "Elias University Emergency Hospital";
                adminElias.EmployerId = adminElias.Id;
                adminElias.Country = "Romania";
                adminElias.City = "Bucharest";
                adminElias.County = "Bucharest";
                adminElias.Number = "17";
                adminElias.Street = "Mărăști ";
                adminElias.UserName = "adminElias";
                adminElias.CreationTime = new System.DateTime(2021, 6, 15);
                adminElias.Surname = "Hospital";
                adminElias.Name = "Elias";
                adminElias.LeagueId = leagueId;

                _context.Users.Add(adminElias);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, adminElias.Id, hospitalAdminRole.Id));
                _context.SaveChanges();
            }
        }

        private void CreateHospitalPersonnelUser(Role hospitalPersonnelRole)
        {
            var hospitalPersonnelUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == User.HospitalPersonnelUserName);
            var leagueId = _context.Leagues.FirstOrDefault(league => league.Name == "Bronze").Id;

            if (hospitalPersonnelUser == null)
            {
                hospitalPersonnelUser = User.CreateTenantHospitalPersonnelUser(_tenantId, "personnel1@victorbabes.com");
                hospitalPersonnelUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(hospitalPersonnelUser, "123qwe");
                hospitalPersonnelUser.Id = GetAndUpdateLastUserId();
                hospitalPersonnelUser.EmployerId = _context.Users.FirstOrDefault(u => u.UserName == "adminBabes").Id;
                hospitalPersonnelUser.UserName = "personnel1_babes";
                hospitalPersonnelUser.Surname = "Personnel 1";
                hospitalPersonnelUser.Name = "Victor Babes";
                hospitalPersonnelUser.LeagueId = leagueId;

                _context.Users.Add(hospitalPersonnelUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, hospitalPersonnelUser.Id, hospitalPersonnelRole.Id));
                _context.SaveChanges();

                hospitalPersonnelUser = User.CreateTenantHospitalPersonnelUser(_tenantId, "personnel1@elias.com");
                hospitalPersonnelUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(hospitalPersonnelUser, "123qwe");
                hospitalPersonnelUser.Id = GetAndUpdateLastUserId();
                hospitalPersonnelUser.EmployerId = _context.Users.FirstOrDefault(u => u.UserName == "adminElias").Id;
                hospitalPersonnelUser.UserName = "personnel1_elias";
                hospitalPersonnelUser.Surname = "Personnel 1";
                hospitalPersonnelUser.Name = "Personnel Elias";
                hospitalPersonnelUser.LeagueId = leagueId;

                _context.Users.Add(hospitalPersonnelUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, hospitalPersonnelUser.Id, hospitalPersonnelRole.Id));
                _context.SaveChanges();

                hospitalPersonnelUser = User.CreateTenantHospitalPersonnelUser(_tenantId, "personnel2@elias.com");
                hospitalPersonnelUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(hospitalPersonnelUser, "123qwe");
                hospitalPersonnelUser.Id = GetAndUpdateLastUserId();
                hospitalPersonnelUser.EmployerId = _context.Users.FirstOrDefault(u => u.UserName == "adminElias").Id;
                hospitalPersonnelUser.UserName = "personnel2_elias";
                hospitalPersonnelUser.Surname = "Personnel 2";
                hospitalPersonnelUser.Name = "Personnel Elias";
                hospitalPersonnelUser.LeagueId = leagueId;

                _context.Users.Add(hospitalPersonnelUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, hospitalPersonnelUser.Id, hospitalPersonnelRole.Id));
                _context.SaveChanges();

                hospitalPersonnelUser = User.CreateTenantHospitalPersonnelUser(_tenantId, "personnel3@elias.com");
                hospitalPersonnelUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(hospitalPersonnelUser, "123qwe");
                hospitalPersonnelUser.Id = GetAndUpdateLastUserId();
                hospitalPersonnelUser.EmployerId = _context.Users.FirstOrDefault(u => u.UserName == "adminElias").Id;
                hospitalPersonnelUser.UserName = "personnel3_elias";
                hospitalPersonnelUser.Surname = "Personnel 3";
                hospitalPersonnelUser.Name = "Personnel Elias";
                hospitalPersonnelUser.LeagueId = leagueId;

                _context.Users.Add(hospitalPersonnelUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, hospitalPersonnelUser.Id, hospitalPersonnelRole.Id));
                _context.SaveChanges();

                hospitalPersonnelUser = User.CreateTenantHospitalPersonnelUser(_tenantId, "personnel4@elias.com");
                hospitalPersonnelUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(hospitalPersonnelUser, "123qwe");
                hospitalPersonnelUser.Id = GetAndUpdateLastUserId();
                hospitalPersonnelUser.EmployerId = _context.Users.FirstOrDefault(u => u.UserName == "adminElias").Id;
                hospitalPersonnelUser.UserName = "personnel4_elias";
                hospitalPersonnelUser.Surname = "Personnel 4";
                hospitalPersonnelUser.Name = "Personnel Elias";
                hospitalPersonnelUser.LeagueId = leagueId;

                _context.Users.Add(hospitalPersonnelUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, hospitalPersonnelUser.Id, hospitalPersonnelRole.Id));
                _context.SaveChanges();


                hospitalPersonnelUser = User.CreateTenantHospitalPersonnelUser(_tenantId, "personnel2@victorbabes.com");
                hospitalPersonnelUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(hospitalPersonnelUser, "123qwe");
                hospitalPersonnelUser.Id = GetAndUpdateLastUserId();
                hospitalPersonnelUser.EmployerId = _context.Users.FirstOrDefault(u => u.UserName == "adminBabes").Id;
                hospitalPersonnelUser.UserName = "personnel2_babes";
                hospitalPersonnelUser.Surname = "Personnel 2";
                hospitalPersonnelUser.Name = "Victor Babes";
                hospitalPersonnelUser.LeagueId = leagueId;


                _context.Users.Add(hospitalPersonnelUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, hospitalPersonnelUser.Id, hospitalPersonnelRole.Id));
                _context.SaveChanges();


                hospitalPersonnelUser = User.CreateTenantHospitalPersonnelUser(_tenantId, "personnel3@victorbabes.com");
                hospitalPersonnelUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(hospitalPersonnelUser, "123qwe");
                hospitalPersonnelUser.Id = GetAndUpdateLastUserId();
                hospitalPersonnelUser.EmployerId = _context.Users.FirstOrDefault(u => u.UserName == "adminBabes").Id;
                hospitalPersonnelUser.UserName = "personnel3_babes";
                hospitalPersonnelUser.Surname = "Personnel 3";
                hospitalPersonnelUser.Name = "Victor Babes";
                hospitalPersonnelUser.LeagueId = leagueId;

                _context.Users.Add(hospitalPersonnelUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, hospitalPersonnelUser.Id, hospitalPersonnelRole.Id));
                _context.SaveChanges();


                hospitalPersonnelUser = User.CreateTenantHospitalPersonnelUser(_tenantId, "personnel1@spitalmilitar.com");
                hospitalPersonnelUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(hospitalPersonnelUser, "123qwe");
                hospitalPersonnelUser.Id = GetAndUpdateLastUserId();
                hospitalPersonnelUser.EmployerId = _context.Users.FirstOrDefault(u => u.UserName == User.HospitalAdminUserName).Id;
                hospitalPersonnelUser.UserName = "personnel1_militar";
                hospitalPersonnelUser.Surname = "Personnel 1";
                hospitalPersonnelUser.Name = "Militar";
                hospitalPersonnelUser.LeagueId = leagueId;

                _context.Users.Add(hospitalPersonnelUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, hospitalPersonnelUser.Id, hospitalPersonnelRole.Id));
                _context.SaveChanges();


                hospitalPersonnelUser = User.CreateTenantHospitalPersonnelUser(_tenantId, "personnel1@boliinfectioasecluj.com");
                hospitalPersonnelUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(hospitalPersonnelUser, "123qwe");
                hospitalPersonnelUser.Id = GetAndUpdateLastUserId();
                hospitalPersonnelUser.EmployerId = _context.Users.FirstOrDefault(u => u.UserName == "adminBoliInfectioaseCluj").Id;
                hospitalPersonnelUser.UserName = "personnel1_boliinfectioasecluj";
                hospitalPersonnelUser.Surname = "Personnel 1";
                hospitalPersonnelUser.Name = "Infectioase Cluj";
                hospitalPersonnelUser.LeagueId = leagueId;

                _context.Users.Add(hospitalPersonnelUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, hospitalPersonnelUser.Id, hospitalPersonnelRole.Id));
                _context.SaveChanges();


                hospitalPersonnelUser = User.CreateTenantHospitalPersonnelUser(_tenantId, "personnel1@mavromati.com");
                hospitalPersonnelUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(hospitalPersonnelUser, "123qwe");
                hospitalPersonnelUser.Id = GetAndUpdateLastUserId();
                hospitalPersonnelUser.EmployerId = _context.Users.FirstOrDefault(u => u.UserName == "adminMavromati").Id;
                hospitalPersonnelUser.UserName = "personnel1_mavromati";
                hospitalPersonnelUser.Surname = "Personnel 1";
                hospitalPersonnelUser.Name = "Mavromati";
                hospitalPersonnelUser.LeagueId = leagueId;

                _context.Users.Add(hospitalPersonnelUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, hospitalPersonnelUser.Id, hospitalPersonnelRole.Id));
                _context.SaveChanges();


                hospitalPersonnelUser = User.CreateTenantHospitalPersonnelUser(_tenantId, "personnel1@urgentabrasov.com");
                hospitalPersonnelUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(hospitalPersonnelUser, "123qwe");
                hospitalPersonnelUser.Id = GetAndUpdateLastUserId();
                hospitalPersonnelUser.EmployerId = _context.Users.FirstOrDefault(u => u.UserName == "adminUrgentaBrasvov").Id;
                hospitalPersonnelUser.UserName = "personnel1_urgentabrasov";
                hospitalPersonnelUser.Surname = "Personnel 1";
                hospitalPersonnelUser.Name = "Urgente Brasov";
                hospitalPersonnelUser.LeagueId = leagueId;

                _context.Users.Add(hospitalPersonnelUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, hospitalPersonnelUser.Id, hospitalPersonnelRole.Id));
                _context.SaveChanges();


                hospitalPersonnelUser = User.CreateTenantHospitalPersonnelUser(_tenantId, "personnel1@urgentacluj.com");
                hospitalPersonnelUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(hospitalPersonnelUser, "123qwe");
                hospitalPersonnelUser.Id = GetAndUpdateLastUserId();
                hospitalPersonnelUser.EmployerId = _context.Users.FirstOrDefault(u => u.UserName == "adminUrgentaCluj").Id;
                hospitalPersonnelUser.UserName = User.HospitalPersonnelUserName;
                hospitalPersonnelUser.Surname = "Personnel 1";
                hospitalPersonnelUser.Name = "Urgente Cluj";
                hospitalPersonnelUser.LeagueId = leagueId;

                _context.Users.Add(hospitalPersonnelUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, hospitalPersonnelUser.Id, hospitalPersonnelRole.Id));
                _context.SaveChanges();
            }
        }

        private void CreateCenterPersonnelUser(Role centerPersonnelRole)
        {
            var centerPersonnelUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == User.CenterPersonnelUserName);
            var leagueId = _context.Leagues.FirstOrDefault(league => league.Name == "Bronze").Id;

            if (centerPersonnelUser == null)
            {
                centerPersonnelUser = User.CreateTenantCenterPersonnelUser(_tenantId, "personnel1@reginamaria.com");
                centerPersonnelUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(centerPersonnelUser, "123qwe");
                centerPersonnelUser.Id = GetAndUpdateLastUserId();
                centerPersonnelUser.EmployerId = _context.Users.FirstOrDefault(u => u.UserName == "adminReginaMaria").Id;
                centerPersonnelUser.UserName = "personnel1_reginamaria";
                centerPersonnelUser.Surname = "Personnel 1";
                centerPersonnelUser.Name = "Regina Maria";
                centerPersonnelUser.LeagueId = leagueId;
                _context.Users.Add(centerPersonnelUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, centerPersonnelUser.Id, centerPersonnelRole.Id));
                _context.SaveChanges();


                centerPersonnelUser = User.CreateTenantCenterPersonnelUser(_tenantId, "personnel2@reginamaria.com");
                centerPersonnelUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(centerPersonnelUser, "123qwe");
                centerPersonnelUser.Id = GetAndUpdateLastUserId();
                centerPersonnelUser.EmployerId = _context.Users.FirstOrDefault(u => u.UserName == "adminReginaMaria").Id;
                centerPersonnelUser.UserName = "personnel2_reginamaria";
                centerPersonnelUser.Surname = "Personnel 2";
                centerPersonnelUser.Name = "Regina Maria";
                centerPersonnelUser.LeagueId = leagueId;
      
                _context.Users.Add(centerPersonnelUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, centerPersonnelUser.Id, centerPersonnelRole.Id));
                _context.SaveChanges();


                centerPersonnelUser = User.CreateTenantCenterPersonnelUser(_tenantId, "personnel1@sinevo.com");
                centerPersonnelUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(centerPersonnelUser, "123qwe");
                centerPersonnelUser.Id = GetAndUpdateLastUserId();
                centerPersonnelUser.EmployerId = _context.Users.FirstOrDefault(u => u.UserName == "adminSinevo").Id;
                centerPersonnelUser.UserName = "personnel1_sinevo";
                centerPersonnelUser.Surname = "Personnel 1";
                centerPersonnelUser.Name = "Sinevo";
                centerPersonnelUser.LeagueId = leagueId;
                _context.Users.Add(centerPersonnelUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, centerPersonnelUser.Id, centerPersonnelRole.Id));
                _context.SaveChanges();


                centerPersonnelUser = User.CreateTenantCenterPersonnelUser(_tenantId, "personnel1@cts.com");
                centerPersonnelUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(centerPersonnelUser, "123qwe");
                centerPersonnelUser.Id = GetAndUpdateLastUserId();
                centerPersonnelUser.EmployerId = _context.Users.FirstOrDefault(u => u.UserName == "adminCTS").Id;
                centerPersonnelUser.UserName = "personnel1_cts";
                centerPersonnelUser.Surname = "Personnel 1";
                centerPersonnelUser.Name = "CTS";
                centerPersonnelUser.LeagueId = leagueId;
                _context.Users.Add(centerPersonnelUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, centerPersonnelUser.Id, centerPersonnelRole.Id));
                _context.SaveChanges();


                centerPersonnelUser = User.CreateTenantCenterPersonnelUser(_tenantId, "personnel2@cts.com");
                centerPersonnelUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(centerPersonnelUser, "123qwe");
                centerPersonnelUser.Id = GetAndUpdateLastUserId();
                centerPersonnelUser.EmployerId = _context.Users.FirstOrDefault(u => u.UserName == "adminCTS").Id;
                centerPersonnelUser.UserName = "personnel2_cts";
                centerPersonnelUser.Surname = "Personnel 2";
                centerPersonnelUser.Name = "CTS";
                centerPersonnelUser.LeagueId = leagueId;
                _context.Users.Add(centerPersonnelUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, centerPersonnelUser.Id, centerPersonnelRole.Id));
                _context.SaveChanges();


                centerPersonnelUser = User.CreateTenantCenterPersonnelUser(_tenantId, "personnel3@cts.com");
                centerPersonnelUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(centerPersonnelUser, "123qwe");
                centerPersonnelUser.Id = GetAndUpdateLastUserId();
                centerPersonnelUser.EmployerId = _context.Users.FirstOrDefault(u => u.UserName == "adminCTS").Id;
                centerPersonnelUser.UserName = "personnel3_cts";
                centerPersonnelUser.Surname = "Personnel 3";
                centerPersonnelUser.Name = "CTS";
                centerPersonnelUser.LeagueId = leagueId;
                _context.Users.Add(centerPersonnelUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, centerPersonnelUser.Id, centerPersonnelRole.Id));
                _context.SaveChanges();


                centerPersonnelUser = User.CreateTenantCenterPersonnelUser(_tenantId, "personnel1@ctsbacau.com");
                centerPersonnelUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(centerPersonnelUser, "123qwe");
                centerPersonnelUser.Id = GetAndUpdateLastUserId();
                centerPersonnelUser.EmployerId = _context.Users.FirstOrDefault(u => u.UserName == "adminCTSBacau").Id;
                centerPersonnelUser.UserName = User.CenterPersonnelUserName;
                centerPersonnelUser.Surname = "Personnel 1";
                centerPersonnelUser.Name = "CTS Bacau";
                centerPersonnelUser.LeagueId = leagueId;
                _context.Users.Add(centerPersonnelUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, centerPersonnelUser.Id, centerPersonnelRole.Id));
                _context.SaveChanges();


                centerPersonnelUser = User.CreateTenantCenterPersonnelUser(_tenantId, "personnel1@ctstimisoara.com");
                centerPersonnelUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(centerPersonnelUser, "123qwe");
                centerPersonnelUser.Id = GetAndUpdateLastUserId();
                centerPersonnelUser.EmployerId = _context.Users.FirstOrDefault(u => u.UserName == User.HospitalAdminUserName).Id;
                centerPersonnelUser.Surname = "Personnel 1";
                centerPersonnelUser.Name = "CTS Timisoara";
                centerPersonnelUser.LeagueId = leagueId;
                _context.Users.Add(centerPersonnelUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, centerPersonnelUser.Id, centerPersonnelRole.Id));
                _context.SaveChanges();
            }
        }
       
        private void CreateDonorUser(Role donorRole)
        {
            var donorUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == User.DonorUserName);
            var leagueId = _context.Leagues.FirstOrDefault(league => league.Name == "Bronze").Id;

            if (donorUser == null)
            {
                donorUser = User.CreateTenantDonorUser(_tenantId, "zotaandrei@gmail.com");
                donorUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(donorUser, "123qwe");
                donorUser.Surname = "Zota";
                donorUser.Name = "Andrei";
                donorUser.Id = 1990305329641;
                donorUser.UserName = "andreiz";
                donorUser.CreationTime = new System.DateTime(2021, 1, 2);
                donorUser.BloodType = BloodTypes.BNegative;
                donorUser.LeagueId = 2;
                donorUser.Points = 40;

                _context.Users.Add(donorUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, donorUser.Id, donorRole.Id));
                _context.SaveChanges();


                donorUser = User.CreateTenantDonorUser(_tenantId, "istratemadalinav@gmail.com");
                donorUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(donorUser, "123qwe");
                donorUser.Surname = "Istrate";
                donorUser.Name = "Madalina";
                donorUser.Id = 2990407460021;
                donorUser.CreationTime = new System.DateTime(2021, 1, 2);
                donorUser.BloodType = BloodTypes.ANegative;
                donorUser.UserName = "madaist";
                donorUser.LeagueId = 4;
                donorUser.Points = 145;
                _context.Users.Add(donorUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, donorUser.Id, donorRole.Id));
                _context.SaveChanges();


                donorUser = User.CreateTenantDonorUser(_tenantId, "popescumaria@gmail.com");
                donorUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(donorUser, "123qwe");
                donorUser.Surname = "Popescu";
                donorUser.Name = "Maria";
                donorUser.Id = 2991230465121;
                donorUser.CreationTime = new System.DateTime(2021, 1, 2);
                donorUser.UserName = "popescumaria";
                donorUser.BloodType = BloodTypes.CPositive;
                donorUser.LeagueId = 4;
                donorUser.Points = 140;
                _context.Users.Add(donorUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, donorUser.Id, donorRole.Id));
                _context.SaveChanges();


                donorUser = User.CreateTenantDonorUser(_tenantId, "iongheorghe@gmail.com");
                donorUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(donorUser, "123qwe");
                donorUser.Surname = "Ion";
                donorUser.Name = "Gheorghe";
                donorUser.Id = 1991207165828;
                donorUser.CreationTime = new System.DateTime(2021, 2, 2);
                donorUser.UserName = "ion.g";
                donorUser.BloodType = BloodTypes.ABNegative;
                donorUser.LeagueId = 4;
                donorUser.Points = 145;
                _context.Users.Add(donorUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, donorUser.Id, donorRole.Id));
                _context.SaveChanges();


                donorUser = User.CreateTenantDonorUser(_tenantId, "mihaialina@gmail.com");
                donorUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(donorUser, "123qwe");
                donorUser.Surname = "Mihai";
                donorUser.Name = "Alina";
                donorUser.Id = 2951208165828;
                donorUser.CreationTime = new System.DateTime(2021, 3, 2);
                donorUser.UserName = "alinam";
                donorUser.LeagueId = leagueId;
                _context.Users.Add(donorUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, donorUser.Id, donorRole.Id));
                _context.SaveChanges();


                donorUser = User.CreateTenantDonorUser(_tenantId, "musatgabriela@gmail.com");
                donorUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(donorUser, "123qwe");
                donorUser.Surname = "Musat";
                donorUser.Name = "Gabriela";
                donorUser.Id = 2961202115227;
                donorUser.CreationTime = new System.DateTime(2021, 4, 2);
                donorUser.UserName = "gabimusat";
                donorUser.BloodType = BloodTypes.APositive;
                donorUser.LeagueId = 2;
                donorUser.Points = 40;
                _context.Users.Add(donorUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, donorUser.Id, donorRole.Id));
                _context.SaveChanges();


                donorUser = User.CreateTenantDonorUser(_tenantId, "dinuionuta@gmail.com");
                donorUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(donorUser, "123qwe");
                donorUser.Surname = "Dinu";
                donorUser.Name = "Ionut";
                donorUser.Id = 1981205115227;
                donorUser.CreationTime = new System.DateTime(2021, 4, 2);
                donorUser.UserName = "ionutdinu";
                donorUser.LeagueId = leagueId;
                _context.Users.Add(donorUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, donorUser.Id, donorRole.Id));
                _context.SaveChanges();


                donorUser = User.CreateTenantDonorUser(_tenantId, "mironadrian@gmail.com");
                donorUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(donorUser, "123qwe");
                donorUser.Surname = "Miron";
                donorUser.Name = "Adrian";
                donorUser.Id = 1930419125887;
                donorUser.CreationTime = new System.DateTime(2021, 5, 2);
                donorUser.UserName = "adrianmiron";
                donorUser.LeagueId = leagueId;
                _context.Users.Add(donorUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, donorUser.Id, donorRole.Id));
                _context.SaveChanges();


                donorUser = User.CreateTenantDonorUser(_tenantId, "neagumaria@gmail.com");
                donorUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(donorUser, "123qwe");
                donorUser.Surname = "Neagu";
                donorUser.Name = "Maria";
                donorUser.Id = 2970421258871;
                donorUser.CreationTime = new System.DateTime(2021, 6, 2);
                donorUser.LeagueId = leagueId;

                _context.Users.Add(donorUser);
                _context.SaveChanges();
                _context.UserRoles.Add(new UserRole(_tenantId, donorUser.Id, donorRole.Id));
                _context.SaveChanges();
            }
        }
    }
}
