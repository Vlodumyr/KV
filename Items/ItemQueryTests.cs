using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using InventoryСontrol.Application.CQRS.Items.Queries;
using InventoryСontrol.Application.CQRS.Items.Views;
using InventoryСontrol.Domain;
using Moq;
using Xunit;

namespace InventoryСontrol.Tests.Items
{
    public class ItemQueryTests : IClassFixture<InventoryСontrolContextFixture>
    {
        private readonly IFixture _fixture = new Fixture();
        private readonly IItemQuery _iItemQuery;
        private readonly InventoryСontrolContextFixture _inventoryСontrolContextFixture;
        private readonly IMapper _mapper;

        public ItemQueryTests()
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

            _iItemQuery = new ItemQuery(_inventoryСontrolContextFixture.context, _mapper);
        }

        [Fact]
        public async Task GetAllItemsAsync_ShouldNotBeNull()
        {
            var result = await _iItemQuery.GetAllItemsAsync();

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task SearchAsync_ShouldBeExpected()
        {
            var searchItemName = "Amogus";

            var numberSearchItem = 1;

            var item1 = _fixture.Build<Item>()
                .With(i => i.Name, searchItemName)
                .Create();

            var item2 = _fixture.Build<Item>()
                .Create();

            var item3 = _fixture.Build<Item>()
                .Create();

            var items = new List<Item> { item1, item2, item3 };

            await _inventoryСontrolContextFixture.InitItemsAsync(items);

            var result = await _iItemQuery.SearchAsync(searchItemName);

            result.Should().NotBeNull();
            result.Count().Should().Be(numberSearchItem);
            result.Select(i => i.ItemId.Equals(item1.ItemId)).Count().Should().Be(numberSearchItem);
        }

        [Fact]
        public async Task GetItemsByFilterAndSortingAsync_ShouldBeExpected()
        {
            var item1 = _fixture.Build<Item>()
                .With(i => i.Cost, 10)
                .With(i => i.Amount, 10)
                .With(i => i.Name, "ZakaZaka")
                .Create();

            var item2 = _fixture.Build<Item>()
                .With(i => i.Cost, 100)
                .With(i => i.Amount, 100)
                .Create();

            var item3 = _fixture.Build<Item>()
                .With(i => i.Cost, 999999)
                .With(i => i.Amount, 99999)
                .Create();

            var items = new List<Item> { item1, item2, item3 };

            await _inventoryСontrolContextFixture.InitItemsAsync(items);

            var result = await _iItemQuery.GetItemsByFilterAndSortingAsync(
                "ZakaZaka", 10, 100, item1.Categories.Select(i => i.Category.Name).ToList(), false);

            var expectedView = _mapper.Map<ItemView>(item1);

            result.Should().NotBeNull();
            result.Select(i => i.Name).Should().Equal(expectedView.Name);
        }
    }
}