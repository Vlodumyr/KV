using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using InventoryСontrol.Application.CQRS.Categories.Commands;
using InventoryСontrol.Application.CQRS.Categories.Views;
using InventoryСontrol.Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace InventoryСontrol.Tests.Categories
{
    public class CategoryCommandTests : IClassFixture<InventoryСontrolContextFixture>
    {
        private readonly IFixture _fixture = new Fixture();
        private readonly ICategoryCommand _iCategoryCommand;
        private readonly InventoryСontrolContextFixture _inventoryСontrolContextFixture;
        private readonly IMapper _mapper;

        public CategoryCommandTests()
        {
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customizations.Add(new PersonExternalIdGenerator());

            _inventoryСontrolContextFixture = new Mock<InventoryСontrolContextFixture>().Object;

            _mapper = new Mapper(new MapperConfiguration(cfg =>
                cfg.CreateMap<Category, CategoryView>()));

            _iCategoryCommand = new CategoryCommand(_inventoryСontrolContextFixture.context, _mapper);
        }

        [Fact]
        public async Task UpdateAsync_ShouldBeExpected()
        {
            var categoryId = Guid.NewGuid();
            var category = _fixture.Build<Category>()
                .With(i => i.CategoryId, categoryId)
                .With(i => i.Name, "aaa")
                .Create();

            await _inventoryСontrolContextFixture.InitCategoresAsync(new List<Category> { category });

            var categoryExpected = _fixture.Build<Category>()
                .With(i => i.CategoryId, categoryId)
                .With(i => i.Name, "bbb")
                .Create();

            var result = await _iCategoryCommand.UpdateAsync(categoryId, "bbb");

            result.Should().NotBeNull();
            result.CategoryId.Should().Be(categoryExpected.CategoryId);
            result.Name.Should().Be(categoryExpected.Name);
        }

        [Fact]
        public async Task AddAsync_ShouldBeExpected()
        {
            var categoryName = "ccc";

            await _iCategoryCommand.AddAsync(categoryName);

            var result = await _inventoryСontrolContextFixture.context.Categories
                .FirstOrDefaultAsync(i => i.Name.Equals(categoryName));

            result.Should().NotBeNull();
            result.Name.Should().Be(categoryName);
        }
    }
}