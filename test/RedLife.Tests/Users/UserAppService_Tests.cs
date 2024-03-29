﻿using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using Abp.Application.Services.Dto;
using RedLife.Users;
using RedLife.Users.Dto;

namespace RedLife.Tests.Users
{
    public class UserAppService_Tests : RedLifeTestBase
    {
        private readonly IUserAppService _userAppService;

        public UserAppService_Tests()
        {
            _userAppService = Resolve<IUserAppService>();
        }

        [Fact]
        public async Task GetUsers_Test()
        {
            // Act
            var output = await _userAppService.GetAllAsync(new PagedUserResultRequestDto{MaxResultCount=20, SkipCount=0} );

            // Assert
            output.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task CreateUser_Test()
        {
            // Act
            await _userAppService.CreateAsync(
                new CreateUserDto
                {
                    EmailAddress = "redlife@yahoo.com",
                    IsActive = true,
                    Name = "Red",
                    Surname = "Life",
                    Password = "123qwe",
                    UserName = "red.life"
                });

            await UsingDbContextAsync(async context =>
            {
                var johnNashUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "red.life");
                johnNashUser.ShouldNotBeNull();
            });
        }
    }
}
