using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.ObjectMapping;
using RedLife.Authorization;
using RedLife.Authorization.Users;
using RedLife.Core.Donations;
using RedLife.Core.PdfHelper;
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
                (roleName == Tenants.CenterPersonnel && entity.CenterId == currentUser.EmployerId) ||
                (roleName == Tenants.CenterAdmin && entity.CenterId == currentUser.Id))
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
            var currentUser = _userRepository.Get(AbpSession.UserId ?? 0);
            var roleName = _userManager.GetCurrentUserRoleAsync(currentUser);

            List<DonationDto> donationDtoOutput = new List<DonationDto>();
            var filteredDonations = CreateFilteredQuery(input).ToList();

            if (roleName == Tenants.Admin)
            {
                donationDtoOutput = _objectMapper.Map<List<DonationDto>>(filteredDonations.ToList());
            }
            else if (roleName == Tenants.Donor)
            {
                donationDtoOutput = _objectMapper.Map<List<DonationDto>>(filteredDonations.
                                            Where(x => x.DonorId == currentUser.Id).ToList());
            }
            else if (roleName == Tenants.CenterPersonnel)
            {
                donationDtoOutput = ObjectMapper.Map<List<DonationDto>>(filteredDonations
                                            .Where(x => x.CenterId == currentUser.EmployerId).ToList());
            }
            else if (roleName == Tenants.CenterAdmin)
            {
                donationDtoOutput = ObjectMapper.Map<List<DonationDto>>(filteredDonations
                                            .Where(x => x.CenterId == currentUser.Id).ToList());
            }
            

            return new PagedResultDto<DonationDto>
            {
                Items = donationDtoOutput.OrderByDescending(donation => donation.Date).ToList(),
                TotalCount = donationDtoOutput.Count
            };
        }

        [AbpAuthorize(PermissionNames.Donations_Update)]
        public override async Task<DonationDto> UpdateAsync(UpdateDonationDto input)
        {
            var entity = _donationRepository.Get(input.Id);
            var currentUser = _userRepository.Get(AbpSession.UserId ?? 0);
            var roleName = _userManager.GetCurrentUserRoleAsync(currentUser);

            if ((roleName == Tenants.Admin) ||
                (roleName == Tenants.Donor && entity.DonorId == AbpSession.UserId) ||
                (roleName == Tenants.CenterAdmin && entity.CenterId == currentUser.Id) ||
                (roleName == Tenants.CenterPersonnel && entity.CenterId == currentUser.EmployerId))
            {
                if (input.MedicalTestsResult != null)
                {
                    PDFUtils.ConvertToPdf(input.MedicalTestsResult, input.Id);
                }
                return await base.UpdateAsync(input);
            }
            else
            {
                throw new Exception("User not authorized to edit the donation");
            }
        }

        [AbpAuthorize(PermissionNames.Donations_Create)]
        public override async Task<DonationDto> CreateAsync(CreateDonationDto input)
        {
            input.Id = input.DonorId + input.Date.Replace("-", "");
        
            var donor = _userRepository.Get(input.DonorId);
            if(donor.BloodType == null)
            {
                donor.BloodType = input.BloodType;
                _userRepository.Update(donor);
            }

            if (input.MedicalTestsResult != null)
            {
                PDFUtils.ConvertToPdf(input.MedicalTestsResult, input.Id);
            }
            return await base.CreateAsync(input);
        }

        [AbpAuthorize(PermissionNames.Donations_Delete)]
        public override Task DeleteAsync(EntityDto<string> input)
        {
            return base.DeleteAsync(input);
        }

        protected override IQueryable<Donation> CreateFilteredQuery(PagedDonationResultRequestDto input)
        {
            return Repository.GetAll()
                             .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Donor.Surname.Contains(input.Keyword)
                                || x.Id.Contains(input.Keyword)
                                || x.Donor.Name.Contains(input.Keyword)
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
