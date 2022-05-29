using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using InventoryСontrol.Api.Persistence;
using InventoryСontrol.Application.CQRS.UserAccounts.Role.Commands;
using InventoryСontrol.Application.CQRS.UserAccounts.Views;
using InventoryСontrol.Domain;
using InventoryСontrol.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace InventoryСontrol.Tests.UserAccounts.Role
{
    public class UserRoleCommandTests : IClassFixture<InventoryСontrolContextFixture>
    {
        private readonly IFixture _fixture = new Fixture();
        private readonly InventoryСontrolContextFixture _inventoryСontrolContextFixture;
        private readonly IUserRoleCommand _iUserRoleCommand;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UserRoleCommandTests()
        {
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customizations.Add(new PersonExternalIdGenerator());

            _inventoryСontrolContextFixture = new Mock<InventoryСontrolContextFixture>().Object;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserAccountRoleView>()
                .ForMember(dest => dest.Roles, src => src.Ignore()));

            _mapper = new Mapper(config);

            _userManager = MockUserManager.UserManager<User>();

            _iUserRoleCommand = new UserRoleCommand(_userManager, _inventoryСontrolContextFixture.context, _mapper);
        }

        [Fact]
        public async Task AddUserRoleAsync_ShouldBeExpected()
        {
            var attachRole = Roles.Admin.ToString();
            var initUser = _fixture.Build<User>()
                .Create();

            await _inventoryСontrolContextFixture.InitUsersAsync(new List<User> { initUser });

            var user = await _iUserRoleCommand.AddUserRoleAsync(Guid.Parse(initUser.Id), attachRole);

            user.Should().NotBeNull();
            user.Email.Should().Be(initUser.Email);
            user.Id.Should().Be(initUser.Id);
        }

        [Fact]
        public async Task RemoveUserRoleAsync_ShouldBeExpected()
        {
            var removeRole = Roles.Admin.ToString();
            var initUser = _fixture.Build<User>()
                .Create();

            await _inventoryСontrolContextFixture.InitUsersAsync(new List<User> { initUser });

            var user = await _iUserRoleCommand.RemoveUserRoleAsync(Guid.Parse(initUser.Id), removeRole);

            user.Should().NotBeNull();
            user.Email.Should().Be(initUser.Email);
            user.Id.Should().Be(initUser.Id);
        }
    }
}