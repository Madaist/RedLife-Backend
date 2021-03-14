using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using RedLife.Authorization.Users;

namespace RedLife.Users.Dto
{
    [AutoMapTo(typeof(User))]
    public class CreateUserDto : IShouldNormalize
    {
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public string[] RoleNames { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }


        public long? SocialSecurityNumber { get; set; }
        public string InstitutionName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string County { get; set; }
        public string Number { get; set; }
        public string Street { get; set; }
        public long? EmployerId { get; set; }


        public void Normalize()
        {
            if (RoleNames == null)
            {
                RoleNames = new string[0];
            }
        }
    }
}
