using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.ObjectMapping;
using Microsoft.AspNetCore.Authorization;
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

        public override async Task<AppointmentDto> GetAsync(EntityDto<int> input)
        {
            var entity = _appointmentRepository.Get(input.Id);
            var currentUser = _userRepository.Get(AbpSession.UserId ?? 0);
            var roleName = _userManager.GetCurrentUserRoleAsync(currentUser);

            if ((roleName == Tenants.Donor && entity.DonorId == AbpSession.UserId) ||
                (roleName == Tenants.Admin) ||
                (roleName.Contains("Center") && entity.CenterId == currentUser.EmployerId))
            {
                return await base.GetAsync(input);
            }
            else
            {
                return null;
            }
        }

        public override async Task<PagedResultDto<AppointmentDto>> GetAllAsync(PagedAppointmentResultRequestDto input)
        {
            var filteredAppointments = CreateFilteredQuery(input).ToList();
            var currentUser = _userRepository.Get(AbpSession.UserId ?? 0);
            var roleName =  _userManager.GetCurrentUserRoleAsync(currentUser);

            if (roleName == Tenants.Donor)
            {
                var appointmentDtoOutput = _objectMapper.Map<List<AppointmentDto>>(filteredAppointments.
                                           Where(x => x.DonorId == currentUser.Id).ToList());
                return new PagedResultDto<AppointmentDto>
                {
                    Items = appointmentDtoOutput,
                    TotalCount = appointmentDtoOutput.Count
                };
            }
            else if (roleName == Tenants.Admin)
            {
                return await base.GetAllAsync(input);
            }
            else if (roleName.Contains("Center"))
            {
                var appointmentDtoOutput = ObjectMapper.Map<List<AppointmentDto>>(_appointmentRepository.GetAll().
                                           Where(x => x.CenterId == currentUser.EmployerId).ToList());
                return new PagedResultDto<AppointmentDto>
                {
                    Items = appointmentDtoOutput,
                    TotalCount = appointmentDtoOutput.Count
                };
            }
            else
            {
                return null;
            }
        }

        public override async Task<AppointmentDto> UpdateAsync(UpdateAppointmentDto input)
        {
            var entity = _appointmentRepository.Get(input.Id);
            var currentUser = _userRepository.Get(AbpSession.UserId ?? 0);
            var roleName = _userManager.GetCurrentUserRoleAsync(currentUser);

            if ((roleName == "Admin") ||
                (roleName == "Donor" && entity.DonorId == AbpSession.UserId) ||
                (roleName == "CenterPersonnel" && entity.CenterId == currentUser.EmployerId))
            {
                return await base.UpdateAsync(input);
            }
            else
            {
                throw new Exception("Not authorized");
            }
        }

        [AbpAuthorize(PermissionNames.Appointments_Create)]
        public override async Task<AppointmentDto> CreateAsync(CreateAppointmentDto input)
        {
            return await base.CreateAsync(input);

        }

        protected override IQueryable<Appointment> CreateFilteredQuery(PagedAppointmentResultRequestDto input)
        {
            return Repository.GetAll()
                             .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Donor.UserName.Contains(input.Keyword)
                                      || x.Center.InstitutionName.Contains(input.Keyword)
                                      || x.Date.ToString().Contains(input.Keyword));
        }

        protected override IQueryable<Appointment> ApplySorting(IQueryable<Appointment> query, PagedAppointmentResultRequestDto input)
        {
            return query.OrderByDescending(r => r.Date);
        }

    }
}
