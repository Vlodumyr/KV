using System;
using System.Threading.Tasks;
using AutoMapper;
using InventoryСontrol.Application.CQRS.Categories.Views;
using InventoryСontrol.Application.Extensions;
using InventoryСontrol.Domain;
using InventoryСontrol.Storage.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryСontrol.Application.CQRS.Categories.Commands
{
    public sealed class CategoryCommand : ICategoryCommand
    {
        private static InventoryСontrolContext _context;
        private static IMapper _mapper;

        public CategoryCommand(
            InventoryСontrolContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CategoryView> AddAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Не валидное значение: name");

            if (await _context.TryGetCategoryAsync(name)) throw new ArgumentException("Такая категория уже существует");

            await _context.Categories.AddAsync(Category.Create(name));

            await _context.SaveChangesAsync();

            var category = await _context.Categories
                .FirstOrDefaultAsync(i => i.Name.Equals(name));

            return _mapper.Map<CategoryView>(category);
        }

        public async Task<CategoryView> UpdateAsync(
            Guid categoryId,
            string name)
        {
            if (!await _context.TryGetCategoryAsync(categoryId)) throw new ArgumentNullException("Категория не найден");

            var category = await _context.Categories
                .FindAsync(categoryId);

            if (!string.IsNullOrWhiteSpace(name)) category.Update(name);

            await _context.SaveChangesAsync();

            return _mapper.Map<CategoryView>(category);
        }
    }
}