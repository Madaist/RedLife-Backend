using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.ObjectMapping;
using RedLife.Application.Transfusions.Dto;
using RedLife.Authorization;
using RedLife.Authorization.Users;
using RedLife.Core.Donations;
using RedLife.Core.EmailSender;
using RedLife.Core.Transfusions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RedLife.Authorization.Roles.StaticRoleNames;

namespace RedLife.Application.Transfusions
{
    public class TransfusionAppService : AsyncCrudAppService<Transfusion, TransfusionDto, string, PagedTransfusionResultRequestDto, CreateTransfusionDto, UpdateTransfusionDto>,
                                         ITransfusionAppService
    {
        private readonly IRepository<Transfusion, string> _transfusionRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Donation, string> _donationRepository;
        private readonly UserManager _userManager;
        private readonly IObjectMapper _objectMapper;
        private readonly IEmailManager _emailManager;

        public TransfusionAppService(IRepository<Transfusion, string> transfusionRepository, IRepository<User, long> userRepository, IRepository<Donation, string> donationRepository, UserManager userManager, IObjectMapper objectMapper, IEmailManager emailManager) : base(transfusionRepository)
        {
            _transfusionRepository = transfusionRepository;
            _userRepository = userRepository;
            _donationRepository = donationRepository;
            _userManager = userManager;
            _objectMapper = objectMapper;
            _emailManager = emailManager;

        }

        [AbpAuthorize(PermissionNames.Transfusions_Get)]
        public override async Task<TransfusionDto> GetAsync(EntityDto<string> input)
        {
            var entity = _transfusionRepository.Get(input.Id);
            var currentUser = _userRepository.Get(AbpSession.UserId ?? 0);
            var roleName = _userManager.GetCurrentUserRoleAsync(currentUser);

            if ((roleName == Tenants.Donor && entity.Donation.DonorId == AbpSession.UserId) ||
                (roleName == Tenants.Admin) ||
                (roleName == Tenants.HospitalAdmin && entity.HospitalId == currentUser.Id) ||
                (roleName == Tenants.HospitalPersonnel && entity.HospitalId == currentUser.EmployerId))
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
            var currentUser = _userRepository.Get(AbpSession.UserId ?? 0);
            var roleName = _userManager.GetCurrentUserRoleAsync(currentUser);
            List<TransfusionDto> transfusionDtoOutput = new List<TransfusionDto>();

            if (roleName == Tenants.Admin)
            {
                return await base.GetAllAsync(input);
            }
            else
            {
                var filteredTransfusions = CreateFilteredQuery(input).ToList();
                if (roleName == Tenants.Donor)
                {
                    transfusionDtoOutput = _objectMapper.Map<List<TransfusionDto>>(filteredTransfusions.
                                               Where(x => x.Donation.DonorId == currentUser.Id).ToList());
                }
                else if (roleName == Tenants.HospitalAdmin)
                {
                    transfusionDtoOutput = ObjectMapper.Map<List<TransfusionDto>>(filteredTransfusions
                                                .Where(x => x.HospitalId == currentUser.Id).ToList());
                }
                else if (roleName == Tenants.HospitalPersonnel)
                {
                    transfusionDtoOutput = ObjectMapper.Map<List<TransfusionDto>>(filteredTransfusions
                                                .Where(x => x.HospitalId == currentUser.EmployerId).ToList());
                }
            }

            return new PagedResultDto<TransfusionDto>
            {
                Items = transfusionDtoOutput,
                TotalCount = transfusionDtoOutput.Count
            };
        }

        [AbpAuthorize(PermissionNames.Transfusions_Update)]
        public override async Task<TransfusionDto> UpdateAsync(UpdateTransfusionDto input)
        {
            var entity = _transfusionRepository.Get(input.Id);
            var currentUser = _userRepository.Get(AbpSession.UserId ?? 0);
            var roleName = _userManager.GetCurrentUserRoleAsync(currentUser);

            if ((roleName == Tenants.Admin) ||
                (roleName == Tenants.Donor && entity.Donation.DonorId == AbpSession.UserId) ||
                (roleName == Tenants.HospitalAdmin && entity.HospitalId == currentUser.Id) ||
                (roleName == Tenants.HospitalPersonnel && entity.HospitalId == currentUser.EmployerId))
            {
                return await base.UpdateAsync(input);
            }
            else
            {
                throw new Exception("User not authorized to update the transfusion");
            }
        }

        [AbpAuthorize(PermissionNames.Transfusions_Create)]
        public override async Task<TransfusionDto> CreateAsync(CreateTransfusionDto input)
        {
            input.Id = Guid.NewGuid().ToString();

            var donation = _donationRepository.Get(input.DonationId);
            var bloodDonor = _userRepository.Get(donation.DonorId);

            _emailManager.SendMailToBloodDonor(bloodDonor, donation);

            return await base.CreateAsync(input);
        }

        [AbpAuthorize(PermissionNames.Transfusions_Delete)]
        public override Task DeleteAsync(EntityDto<string> input)
        {
            return base.DeleteAsync(input);
        }

        protected override IQueryable<Transfusion> CreateFilteredQuery(PagedTransfusionResultRequestDto input)
        {
            return Repository.GetAll()
                             .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Donation.Donor.Surname.Contains(input.Keyword)
                                      || x.Donation.Donor.Name.Contains(input.Keyword)
                                      || x.Hospital.InstitutionName.Contains(input.Keyword)
                                      || x.Donation.DonorId.ToString().Contains(input.Keyword)
                                      || x.Date.ToString().Contains(input.Keyword));
        }

        protected override IQueryable<Transfusion> ApplySorting(IQueryable<Transfusion> query, PagedTransfusionResultRequestDto input)
        {
            return query.OrderByDescending(r => r.Date);
        }
    }
}
