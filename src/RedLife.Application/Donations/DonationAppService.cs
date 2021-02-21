﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.ObjectMapping;
using RedLife.Authorization;
using RedLife.Authorization.Users;
using RedLife.Core.Donations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RedLife.Authorization.Roles.StaticRoleNames;

namespace RedLife.Application.Donations.Dto
{
    [AbpAuthorize]
    public class DonationAppService : AsyncCrudAppService<Donation, DonationDto, string, PagedDonationResultRequestDto, CreateDonationDto, UpdateDonationDto>,
                                         IDonationAppService
    {
        private readonly IRepository<Donation, string> _donationRepository;
        private readonly IObjectMapper _objectMapper;
        private readonly IRepository<User, long> _userRepository;
        private readonly UserManager _userManager;

        public DonationAppService(IRepository<Donation, string> donationRepository, IRepository<User, long> userRepository, UserManager userManager, IObjectMapper objectMapper) : base(donationRepository)
        {
            _donationRepository = donationRepository;
            _userRepository = userRepository;
            _userManager = userManager;
            _objectMapper = objectMapper;
        }

        [AbpAuthorize(PermissionNames.Donations_Get)]
        public override async Task<DonationDto> GetAsync(EntityDto<string> input)
        {
            var entity = _donationRepository.Get(input.Id);
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
                throw new Exception("Not authorized");
            }
        }


        [AbpAuthorize(PermissionNames.Donations_Get)]
        public override async Task<PagedResultDto<DonationDto>> GetAllAsync(PagedDonationResultRequestDto input)
        {
            var filteredDonations = CreateFilteredQuery(input).ToList();
            var currentUser = _userRepository.Get(AbpSession.UserId ?? 0);
            var roleName = _userManager.GetCurrentUserRoleAsync(currentUser);

            if (roleName == Tenants.Donor)
            {
                var donationDtoOutput = _objectMapper.Map<List<DonationDto>>(filteredDonations.
                                           Where(x => x.DonorId == currentUser.Id).ToList());
                return new PagedResultDto<DonationDto>
                {
                    Items = donationDtoOutput,
                    TotalCount = donationDtoOutput.Count
                };
            }
            else if (roleName == Tenants.Admin)
            {
                var donationDtoOutput = ObjectMapper.Map<List<DonationDto>>(filteredDonations).ToList();
                return new PagedResultDto<DonationDto>
                {
                    Items = donationDtoOutput,
                    TotalCount = donationDtoOutput.Count
                };
            }
            else if (roleName.Contains("Center"))
            {
                var donationDtoOutput = ObjectMapper.Map<List<DonationDto>>(filteredDonations
                                            .Where(x => x.CenterId == currentUser.EmployerId).ToList());
                return new PagedResultDto<DonationDto>
                {
                    Items = donationDtoOutput,
                    TotalCount = donationDtoOutput.Count
                };
            }
            else
            {
                throw new Exception("Not authorized");
            }
        }

        [AbpAuthorize(PermissionNames.Donations_Update)]
        public override async Task<DonationDto> UpdateAsync(UpdateDonationDto input)
        {
            var entity = _donationRepository.Get(input.Id);
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

        [AbpAuthorize(PermissionNames.Donations_Create)]
        public override async Task<DonationDto> CreateAsync(CreateDonationDto input)
        {
            input.Id = input.DonorId + input.Date.Replace("-", "");
            return await base.CreateAsync(input);
        }

        [AbpAuthorize(PermissionNames.Donations_Delete)]
        public override Task DeleteAsync(EntityDto<string> input)
        {
            return base.DeleteAsync(input);
        }

        protected override IQueryable<Donation> CreateFilteredQuery(PagedDonationResultRequestDto input)
        {
            return (IQueryable<Donation>)Repository.GetAll()
                             .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Donor.UserName.Contains(input.Keyword)
                                      || x.Center.InstitutionName.Contains(input.Keyword)
                                      || x.Date.ToString().Contains(input.Keyword)
                                      || x.DonorId.ToString().Contains(input.Keyword));
        }

        protected override IQueryable<Donation> ApplySorting(IQueryable<Donation> query, PagedDonationResultRequestDto input)
        {
            return query.OrderByDescending(r => r.Date);
        }

    }
}
