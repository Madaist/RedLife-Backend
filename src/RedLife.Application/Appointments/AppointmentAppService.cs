using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.AspNetCore.Authorization;
using RedLife.Application.Appointments.Dto;
using RedLife.Authorization;
using RedLife.Authorization.Users;
using RedLife.Core.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedLife.Application.Appointments
{
    [AllowAnonymous]
    public class AppointmentAppService : AsyncCrudAppService<Appointment, AppointmentDto, int, PagedAppointmentResultRequestDto>, IAppointmentAppService
    {
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly UserManager _userManager;

        public AppointmentAppService(IRepository<Appointment> appointmentRepository, IRepository<User, long> userRepository, UserManager userManager) : base(appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
            _userRepository = userRepository;
            _userManager = userManager;

            CreatePermissionName = PermissionNames.Appointment_Create;
        }

        [AbpAuthorize()]
        public override async Task<AppointmentDto> GetAsync(EntityDto<int> input)
        {
            var entity = _appointmentRepository.Get(input.Id);
            var currentUser = _userRepository.Get(AbpSession.UserId ?? 0);
            var roleName = await _userManager.GetCurrentUserRoleAsync(currentUser);

            if ((roleName == "Donor" && entity.DonorId == AbpSession.UserId) ||
                (roleName == "Admin") ||
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
            var currentUser = _userRepository.Get(AbpSession.UserId ?? 0);
            var roleName = await _userManager.GetCurrentUserRoleAsync(currentUser);

            if (roleName == "Donor")
            {
                var appointmentDtoOutput = ObjectMapper.Map<List<AppointmentDto>>(_appointmentRepository.GetAll().
                                           Where(x => x.DonorId == currentUser.Id).ToList());
                return new PagedResultDto<AppointmentDto>
                {
                    Items = appointmentDtoOutput,
                    TotalCount = appointmentDtoOutput.Count
                };
            }
            else if (roleName == "Admin")
            {
                var appointmentDtoOutput = ObjectMapper.Map<List<AppointmentDto>>(base.GetAllAsync(input));
                return new PagedResultDto<AppointmentDto>
                {
                    Items = appointmentDtoOutput,
                    TotalCount = appointmentDtoOutput.Count
                };
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

        public override async Task<AppointmentDto> UpdateAsync(AppointmentDto input)
        {
            var entity = _appointmentRepository.Get(input.Id);
            var currentUser = _userRepository.Get(AbpSession.UserId ?? 0);
            var roleName = await _userManager.GetCurrentUserRoleAsync(currentUser);

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

        public override async Task<AppointmentDto> CreateAsync(AppointmentDto input)
        {
            var currentUser = _userRepository.Get(AbpSession.UserId ?? 0);
            var roleName = await _userManager.GetCurrentUserRoleAsync(currentUser);

            if (roleName == "Donor" || roleName == "Admin")
            {
                return await base.CreateAsync(input);
            }
            else
            {
                throw new Exception("Not authorized");
            }
        }


        protected override IQueryable<Appointment> CreateFilteredQuery(PagedAppointmentResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.DonorId.ToString().Contains(input.Keyword)
                || x.CenterId.ToString().Contains(input.Keyword)
                || x.Date.ToString().Contains(input.Keyword));
        }

        protected override IQueryable<Appointment> ApplySorting(IQueryable<Appointment> query, PagedAppointmentResultRequestDto input)
        {
            return query.OrderByDescending(r => r.Date);
        }

        //[AbpAuthorize(PermissionNames.Appointment_Create)]
        //public int Getrandom()
        //{
        //    return 5;
        //}
    }
}
