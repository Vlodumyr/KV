using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using InventoryСontrol.Application.CQRS.Categories.Queries;
using InventoryСontrol.Application.CQRS.Categories.Views;
using InventoryСontrol.Domain;
using Moq;
using Xunit;

namespace InventoryСontrol.Tests.Categories
{
    public class CategoryQueryTests : IClassFixture<InventoryСontrolContextFixture>
    {
        private readonly IFixture _fixture = new Fixture();
        private readonly ICategoryQuery _iCategoryQuery;
        private readonly InventoryСontrolContextFixture _inventoryСontrolContextFixture;
        private readonly IMapper _mapper;

        public CategoryQueryTests()
        {
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customizations.Add(new PersonExternalIdGenerator());

            _inventoryСontrolContextFixture = new Mock<InventoryСontrolContextFixture>().Object;

            _mapper = new Mapper(new MapperConfiguration(cfg =>
                cfg.CreateMap<Category, CategoryView>()));

            _iCategoryQuery = new CategoryQuery(_inventoryСontrolContextFixture.context, _mapper);
        }

        [Fact]
        public async Task GetAllItemsAsync_ShouldNotBeNull()
        {
            var result = await _iCategoryQuery.GetAllCategoriesAsync();

            result.Should().NotBeNull();
        }
    }
}