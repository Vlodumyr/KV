using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using InventoryСontrol.Application.CQRS.Items.Commands;
using InventoryСontrol.Application.CQRS.Items.Views;
using InventoryСontrol.Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace InventoryСontrol.Tests.Items
{
    public class ItemCommandTests : IClassFixture<InventoryСontrolContextFixture>
    {
        private readonly IFixture _fixture = new Fixture();
        private readonly IItemCommand _iItemCommand;
        private readonly InventoryСontrolContextFixture _inventoryСontrolContextFixture;
        private readonly IMapper _mapper;

        public ItemCommandTests()
        {
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customizations.Add(new PersonExternalIdGenerator());

            _inventoryСontrolContextFixture = new Mock<InventoryСontrolContextFixture>().Object;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Item, ItemView>()
                .ForMember(dest => dest.Categories,
                    dest => dest.MapFrom(src => src.Categories
                        .Select(i => i.Category.Name)
                        .ToArray())));
            _mapper = new Mapper(config);

            _iItemCommand = new ItemCommand(_inventoryСontrolContextFixture.context, _mapper);
        }

        [Fact]
        public async Task UpdateAsync_ShouldBeExpected()
        {
            var itemId = Guid.NewGuid();
            var item = _fixture.Build<Item>()
                .With(i => i.ItemId, itemId)
                .With(i => i.Amount, 10)
                .With(i => i.Cost, 100)
                .With(i => i.Name, "aaa")
                .Create();

            await _inventoryСontrolContextFixture.InitItemsAsync(new List<Item> { item });

            var itemExpected = _fixture.Build<Item>()
                .With(i => i.ItemId, itemId)
                .With(i => i.Amount, 5)
                .With(i => i.Cost, 50)
                .With(i => i.Name, "bbb")
                .Create();

            var result = await _iItemCommand.UpdateAsync(itemId, "bbb", 5, 50);

            result.Should().NotBeNull();
            result.ItemId.Should().Be(itemExpected.ItemId);
            result.Name.Should().Be(itemExpected.Name);
            result.Amount.Should().Be(itemExpected.Amount);
            result.Cost.Should().Be(itemExpected.Cost);
        }

        [Fact]
        public async Task AddItemAsync_ShouldBeExpected()
        {
            var item = _fixture.Build<Item>()
                .With(i => i.Amount, 10)
                .With(i => i.Cost, 100)
                .With(i => i.Name, "ccc")
                .Create();

            await _iItemCommand.AddAsync("ccc", 10, 100);

            var result = await _inventoryСontrolContextFixture.context.Items
                .FirstOrDefaultAsync();

            result.Should().NotBeNull();
            result.Name.Should().Be(item.Name);
            result.Amount.Should().Be(item.Amount);
            result.Cost.Should().Be(item.Cost);
        }

        [Fact]
        public async Task BuyItemsAsync_ShouldBeExpected()
        {
            var itemId = Guid.NewGuid();
            var item = _fixture.Build<Item>()
                .With(i => i.ItemId, itemId)
                .With(i => i.Amount, 10)
                .With(i => i.Cost, 100)
                .With(i => i.Name, "ddd")
                .Create();

            await _inventoryСontrolContextFixture.InitItemsAsync(new List<Item> { item });

            await _iItemCommand.BuyAsync(itemId, 5);

            var itemExpected = _fixture.Build<Item>()
                .With(i => i.ItemId, itemId)
                .With(i => i.Amount, 5)
                .With(i => i.Cost, 100)
                .With(i => i.Name, "ddd")
                .Create();

            var result = await _inventoryСontrolContextFixture.context.Items
                .Where(i => i.ItemId.Equals(item.ItemId))
                .FirstOrDefaultAsync();

            result.Should().NotBeNull();
            result.ItemId.Should().Be(itemExpected.ItemId);
            result.Name.Should().Be(itemExpected.Name);
            result.Amount.Should().Be(itemExpected.Amount);
            result.Cost.Should().Be(itemExpected.Cost);
        }

        [Fact]
        public async Task PreOrderAsync_ShouldBeExpected()
        {
            var itemId = Guid.NewGuid();
            var userName = "Tom";

            var item = _fixture.Build<Item>()
                .With(i => i.ItemId, itemId)
                .With(i => i.Amount, 0)
                .With(i => i.Cost, 100)
                .With(i => i.Name, "xxx")
                .Create();

            var user = _fixture.Build<User>()
                .With(i => i.UserName, userName)
                .Create();

            await _inventoryСontrolContextFixture.InitItemsAsync(new List<Item> { item });
            await _inventoryСontrolContextFixture.InitUsersAsync(new List<User> { user });

            var userId = await _inventoryСontrolContextFixture.context.Users
                .Where(i => i.UserName.Equals(userName))
                .Select(i => i.Id)
                .FirstOrDefaultAsync();

            await _iItemCommand.PreOrderAsync(userId, itemId, 5);

            var result = await _inventoryСontrolContextFixture.context.PreOrders
                .LastOrDefaultAsync();

            result.Should().NotBeNull();
            result.ItemId.Should().Be(itemId);
            result.UserId.Should().Be(userId);
        }

        [Fact]
        public async Task AddCategoryToItemAsync_ShouldBeExpected()
        {
            var itemId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();

            var item = _fixture.Build<Item>()
                .With(i => i.ItemId, itemId)
                .Create();

            var category = _fixture.Build<Category>()
                .With(i => i.CategoryId, categoryId)
                .Create();

            await _inventoryСontrolContextFixture.InitCategoresAsync(new List<Category> { category });
            await _inventoryСontrolContextFixture.InitItemsAsync(new List<Item> { item });

            await _iItemCommand.AddCategoryToItemAsync(itemId, categoryId);

            var result = await _inventoryСontrolContextFixture.context.Items
                .Include(i => i.Categories)
                .FirstOrDefaultAsync(i => i.ItemId.Equals(itemId));

            result.Should().NotBeNull();
            result.Categories.Any(i => i.Category.CategoryId.Equals(category.CategoryId))
                .Should().Be(true);
        }
    }
}