﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.ObjectMapping;
using RedLife.Application.Appointments.Dto;
using RedLife.Authorization;
using RedLife.Authorization.Users;
using RedLife.Core.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RedLife.Authorization.Roles.StaticRoleNames;

namespace RedLife.Application.Appointments
{
    [AbpAuthorize]
    public class AppointmentAppService : AsyncCrudAppService<Appointment, AppointmentDto, int, PagedAppointmentResultRequestDto, CreateAppointmentDto, UpdateAppointmentDto>,
                                         IAppointmentAppService
    {
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly UserManager _userManager;
        private readonly IObjectMapper _objectMapper;

        public AppointmentAppService(IRepository<Appointment> appointmentRepository, IRepository<User, long> userRepository, UserManager userManager, IObjectMapper objectMapper) : base(appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
            _userRepository = userRepository;
            _userManager = userManager;
            _objectMapper = objectMapper;
        }

        [AbpAuthorize(PermissionNames.Appointments_Get)]
        public override async Task<AppointmentDto> GetAsync(EntityDto<int> input)
        {
            var entity = _appointmentRepository.Get(input.Id);
            var currentUser = _userRepository.Get(AbpSession.UserId ?? 0);
            var roleName = _userManager.GetCurrentUserRoleAsync(currentUser);

            if ((roleName == Tenants.Donor && entity.DonorId == AbpSession.UserId) ||
                (roleName == Tenants.Admin) ||
                (roleName == Tenants.CenterPersonnel && entity.CenterId == currentUser.EmployerId) ||
                (roleName == Tenants.CenterAdmin && entity.CenterId == currentUser.Id))
            {
                return await base.GetAsync(input);
            }
            else
            {
                return null;
            }
        }

        [AbpAuthorize(PermissionNames.Appointments_Get)]
        public override async Task<PagedResultDto<AppointmentDto>> GetAllAsync(PagedAppointmentResultRequestDto input)
        {

            var currentUser = _userRepository.Get(AbpSession.UserId ?? 0);
            var roleName = _userManager.GetCurrentUserRoleAsync(currentUser);

            List<AppointmentDto> appointmentDtoOutput = new List<AppointmentDto>();
            var filteredAppointments = CreateFilteredQuery(input).ToList();
            if (roleName == Tenants.Admin)
            {
                appointmentDtoOutput = _objectMapper.Map<List<AppointmentDto>>(filteredAppointments.ToList());
            }
            else if (roleName == Tenants.Donor)
            {
                appointmentDtoOutput = _objectMapper.Map<List<AppointmentDto>>(filteredAppointments.
                                            Where(x => x.DonorId == currentUser.Id).ToList());
            }
            else if (roleName == Tenants.CenterPersonnel)
            {
                appointmentDtoOutput = ObjectMapper.Map<List<AppointmentDto>>(filteredAppointments
                                            .Where(x => x.CenterId == currentUser.EmployerId).ToList());
            }
            else if (roleName == Tenants.CenterAdmin)
            {
                appointmentDtoOutput = ObjectMapper.Map<List<AppointmentDto>>(filteredAppointments
                                            .Where(x => x.CenterId == currentUser.Id).ToList());
            }

            return new PagedResultDto<AppointmentDto>
            {
                Items = appointmentDtoOutput.OrderByDescending(appointment => appointment.Date).ToList(),
                TotalCount = appointmentDtoOutput.Count
            };
        }

        [AbpAuthorize(PermissionNames.Appointments_Update)]
        public override async Task<AppointmentDto> UpdateAsync(UpdateAppointmentDto input)
        {
            var entity = _appointmentRepository.Get(input.Id);
            var currentUser = _userRepository.Get(AbpSession.UserId ?? 0);
            var roleName = _userManager.GetCurrentUserRoleAsync(currentUser);

            if ((roleName == Tenants.Admin) ||
                (roleName == Tenants.Donor && entity.DonorId == AbpSession.UserId) ||
                (roleName == Tenants.CenterPersonnel && entity.CenterId == currentUser.EmployerId) ||
                (roleName == Tenants.CenterAdmin && entity.CenterId == currentUser.Id))
            {
                return await base.UpdateAsync(input);
            }
            else
            {
                throw new Exception("User not authorized to update the appointment");
            }
        }

        [AbpAuthorize(PermissionNames.Appointments_Create)]
        public override async Task<AppointmentDto> CreateAsync(CreateAppointmentDto input)
        {
            return await base.CreateAsync(input);
        }

        [AbpAuthorize(PermissionNames.Appointments_Delete)]
        public override Task DeleteAsync(EntityDto<int> input)
        {
            return base.DeleteAsync(input);
        }

        protected override IQueryable<Appointment> CreateFilteredQuery(PagedAppointmentResultRequestDto input)
        {
            string keyword = null;
            if (input.Keyword != null)
                keyword = input.Keyword.ToLower();
            else keyword = input.Keyword;

            return Repository.GetAll()
                             .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Donor.UserName.Contains(keyword)
                                      || x.Center.InstitutionName.ToLower().Contains(keyword)
                                      || x.Date.ToString().Contains(keyword)
                                      || x.Donor.Surname.ToLower().Contains(keyword)
                                      ||x.Donor.Name.ToLower().Contains(keyword));
        }

        protected override IQueryable<Appointment> ApplySorting(IQueryable<Appointment> query, PagedAppointmentResultRequestDto input)
        {
            return query.OrderByDescending(r => r.Date);
        }

        public Boolean IsAppointmentDateValid(long donorId, DateTime date)
        {
            var userAppointments = _appointmentRepository.GetAll().Where(a => a.DonorId == donorId).ToList();
            foreach(Appointment app in userAppointments)
            {
                var appDate = app.Date.ToLocalTime().ToString("yyyy-MM-dd");
                if (appDate == date.ToLocalTime().ToString("yyyy-MM-dd"))
                    return false;
            }
            return true;
        }

    }
}
