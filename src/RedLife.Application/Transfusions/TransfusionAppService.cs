using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.ObjectMapping;
using RedLife.Application.Transfusions.Dto;
using RedLife.Authorization;
using RedLife.Authorization.Users;
using RedLife.Core.Transfusions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RedLife.Authorization.Roles.StaticRoleNames;

namespace RedLife.Application.Transfusions
{
    public class TransfusionAppService : AsyncCrudAppService<Transfusion, TransfusionDto, string, PagedTransfusionResultRequestDto, CreateTransfusionDto, UpdateTransfusionDto>,
                                         ITransfusionAppService
    {
        private readonly IRepository<Transfusion, string> _transfusionRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly UserManager _userManager;
        private readonly IObjectMapper _objectMapper;

        public TransfusionAppService(IRepository<Transfusion, string> transfusionRepository, IRepository<User, long> userRepository, UserManager userManager, IObjectMapper objectMapper) : base(transfusionRepository)
        {
            _transfusionRepository = transfusionRepository;
            _userRepository = userRepository;
            _userManager = userManager;
            _objectMapper = objectMapper;
        }

        [AbpAuthorize(PermissionNames.Transfusions_Get)]
        public override async Task<TransfusionDto> GetAsync(EntityDto<string> input)
        {
            var entity = _transfusionRepository.Get(input.Id);
            var currentUser = _userRepository.Get(AbpSession.UserId ?? 0);
            var roleName = _userManager.GetCurrentUserRoleAsync(currentUser);

            if ((roleName == Tenants.Donor && entity.Donation.DonorId == AbpSession.UserId) ||
                (roleName == Tenants.Admin) ||
                (roleName.Contains("Center") && entity.Donation.CenterId == currentUser.EmployerId))
            {
                return await base.GetAsync(input);
            }
            else
            {
                return null;
            }
        }

        [AbpAuthorize(PermissionNames.Transfusions_Get)]
        public override async Task<PagedResultDto<TransfusionDto>> GetAllAsync(PagedTransfusionResultRequestDto input)
        {
            var filteredTransfusions = CreateFilteredQuery(input).ToList();
            var currentUser = _userRepository.Get(AbpSession.UserId ?? 0);
            var roleName = _userManager.GetCurrentUserRoleAsync(currentUser);

            if (roleName == Tenants.Donor)
            {
                var transfusionDtoOutput = _objectMapper.Map<List<TransfusionDto>>(filteredTransfusions.
                                           Where(x => x.Donation.DonorId == currentUser.Id).ToList());
                return new PagedResultDto<TransfusionDto>
                {
                    Items = transfusionDtoOutput,
                    TotalCount = transfusionDtoOutput.Count
                };
            }
            else if (roleName == Tenants.Admin)
            {
                var transfusionDtoOutput = ObjectMapper.Map<List<TransfusionDto>>(filteredTransfusions).ToList();
                return new PagedResultDto<TransfusionDto>
                {
                    Items = transfusionDtoOutput,
                    TotalCount = transfusionDtoOutput.Count
                };
            }
            else if (roleName.Contains("Center"))
            {
                var transfusionDtoOutput = ObjectMapper.Map<List<TransfusionDto>>(filteredTransfusions
                                            .Where(x => x.Donation.CenterId == currentUser.EmployerId).ToList());
                return new PagedResultDto<TransfusionDto>
                {
                    Items = transfusionDtoOutput,
                    TotalCount = transfusionDtoOutput.Count
                };
            }
            else
            {
                return null;
            }
        }

        [AbpAuthorize(PermissionNames.Transfusions_Update)]
        public override async Task<TransfusionDto> UpdateAsync(UpdateTransfusionDto input)
        {
            var entity = _transfusionRepository.Get(input.Id);
            var currentUser = _userRepository.Get(AbpSession.UserId ?? 0);
            var roleName = _userManager.GetCurrentUserRoleAsync(currentUser);

            if ((roleName == "Admin") ||
                (roleName == "Donor" && entity.Donation.DonorId == AbpSession.UserId) ||
                (roleName == "CenterAdmin" && entity.Donation.CenterId == currentUser.EmployerId))
            {
                return await base.UpdateAsync(input);
            }
            else
            {
                throw new Exception("Not authorized");
            }
        }

        [AbpAuthorize(PermissionNames.Transfusions_Create)]
        public override async Task<TransfusionDto> CreateAsync(CreateTransfusionDto input)
        {
            return await base.CreateAsync(input);
        }

        [AbpAuthorize(PermissionNames.Transfusions_Delete)]
        public override Task DeleteAsync(EntityDto<string> input)
        {
            return base.DeleteAsync(input);
        }

        protected override IQueryable<Transfusion> CreateFilteredQuery(PagedTransfusionResultRequestDto input)
        {
            return (IQueryable<Transfusion>)Repository.GetAll()
                             .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Donation.Donor.Surname.Contains(input.Keyword)
                                      || x.Donation.Donor.Name.Contains(input.Keyword)
                                      || x.Donation.Center.InstitutionName.Contains(input.Keyword)
                                      || x.Donation.DonorId.ToString().Contains(input.Keyword)
                                      || x.Date.ToString().Contains(input.Keyword));
        }

        protected override IQueryable<Transfusion> ApplySorting(IQueryable<Transfusion> query, PagedTransfusionResultRequestDto input)
        {
            return query.OrderByDescending(r => r.Date);
        }
    }
}
