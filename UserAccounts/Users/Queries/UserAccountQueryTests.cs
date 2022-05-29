using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using InventoryСontrol.Application.CQRS.UserAccounts.Users.Queries;
using InventoryСontrol.Application.CQRS.UserAccounts.Views;
using InventoryСontrol.Domain;
using Moq;
using Xunit;

namespace InventoryСontrol.Tests.UserAccounts.Users.Queries
{
    public class UserAccountQueryTests : IClassFixture<InventoryСontrolContextFixture>
    {
        private readonly IFixture _fixture = new Fixture();
        private readonly InventoryСontrolContextFixture _inventoryСontrolContextFixture;
        private readonly IUserAccountQuery _iUserAccountQuery;
        private readonly IMapper _mapper;

        public UserAccountQueryTests()
        {
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customizations.Add(new PersonExternalIdGenerator());

            _inventoryСontrolContextFixture = new Mock<InventoryСontrolContextFixture>().Object;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserAccountView>());

            _mapper = new Mapper(config);

            _iUserAccountQuery = new UserAccountQuery(_inventoryСontrolContextFixture.context, _mapper);
        }

        [Fact]
        public async Task GetUsersByEmail_ShouldBeExpected()
        {
            var searchEmail = "amogus@gmaul.com";

            var initUser1 = _fixture.Build<User>()
                .With(i => i.Email, searchEmail)
                .Create();

            var initUser2 = _fixture.Build<User>()
                .Create();

            var initUser3 = _fixture.Build<User>()
                .Create();

            var initUsers = new List<User> { initUser1, initUser2, initUser3 };

            await _inventoryСontrolContextFixture.InitUsersAsync(initUsers);

            var result = _iUserAccountQuery.GetUsersByEmail(searchEmail);

            result.Should().NotBeNull();
            result.Select(i => i.Email).Should().Equal(searchEmail);
        }

        [Fact]
        public void GetAll_ShouldNotBeNull()
        {
            var result = _iUserAccountQuery.GetAll();

            result.Should().NotBeNull();
        }
    }
}