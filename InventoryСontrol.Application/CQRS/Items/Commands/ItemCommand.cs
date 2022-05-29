using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventoryСontrol.Application.CQRS.Items.Views;
using InventoryСontrol.Application.Extensions;
using InventoryСontrol.Domain;
using InventoryСontrol.Storage.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryСontrol.Application.CQRS.Items.Commands
{
    public sealed class ItemCommand : IItemCommand
    {
        private static InventoryСontrolContext _context;
        private static IMapper _mapper;

        public ItemCommand(
            InventoryСontrolContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ItemView> UpdateAsync(
            Guid itemId,
            string name,
            int? amount,
            int? cost)
        {
            if (!await _context.TryGetItemAsync(itemId)) throw new ArgumentNullException("Предмет не найден");

            var result = await _context.Items
                .Where(i => i.ItemId.Equals(itemId))
                .Include(i => i.Categories)
                .ThenInclude(i => i.Category)
                .FirstOrDefaultAsync();

            if (!string.IsNullOrWhiteSpace(name)) result.UpdateName(name);

            if (amount.HasValue)
            {
                if (amount <= 0) throw new ArgumentException("Количество предмета должно быть больше 0");

                result.UpdateAmount((int)amount);
            }

            if (cost.HasValue)
            {
                if (cost <= 0) throw new ArgumentException("Цена предмета должно быть больше 0");

                result.UpdateCost((int)cost);
            }

            await _context.SaveChangesAsync();

            return _mapper.Map<ItemView>(result);
        }

        public async Task<ItemView> AddAsync(
            string name,
            int amount,
            int cost)
        {
            if (await _context.TryGetItemAsync(name)) throw new ArgumentException("Такой предмет уже существует");
            if (amount <= 0) throw new ArgumentException("Количество предмета должно быть больше 0");
            if (cost <= 0) throw new ArgumentException("Цена предмета должна быть больше 0");

            _context.Items.Add(Item.Create(name, cost, amount));

            await _context.SaveChangesAsync();

            var item = await _context.Items
                .FirstOrDefaultAsync(i => i.Name.Equals(name));

            return _mapper.Map<ItemView>(item);
        }

        public async Task BuyAsync(
            Guid itemId,
            int amount)
        {
            if (!await _context.TryGetItemAsync(itemId)) throw new ArgumentNullException("Предмет не найден");

            var item = await _context.Items.FindAsync(itemId);

            if (item.Amount < amount) throw new ArgumentException("На складе нет такого количества товара");

            item.UpdateAmount(item.Amount - amount);
            await _context.SaveChangesAsync();
        }

        public async Task PreOrderAsync(
            string UserId,
            Guid itemId,
            int amount)
        {
            if (!await _context.TryGetItemAsync(itemId)) throw new ArgumentNullException("Предмет не найден");

            var item = await _context.Items.FindAsync(itemId);

            if (item.Amount >= amount)
                throw new ArgumentException("Предмет есть на складке, оформить предзаказ не возможно");

            var user = await _context.Users.FirstOrDefaultAsync(i => i.Id.Equals(UserId));

            var preOrder = PreOrder.Create(item, user, amount);

            _context.PreOrders.Add(preOrder);

            await _context.SaveChangesAsync();
        }

        public async Task<ItemView> AddCategoryToItemAsync(
            Guid itemId,
            Guid categoryId)
        {
            if (!await _context.TryGetItemAsync(itemId)) throw new ArgumentNullException("Предмет не найден");

            if (!await _context.TryGetCategoryAsync(categoryId)) throw new ArgumentNullException("Категория не найден");

            var result = await _context.Items
                .Include(i => i.Categories)
                .ThenInclude(i => i.Category)
                .FirstOrDefaultAsync(i => i.ItemId.Equals(itemId));

            if (result.Categories.Any(i => i.CategoryId.Equals(categoryId)))
                throw new ArgumentException("У данного предмета уже есть данная категория");

            var item = await _context.Items.FirstOrDefaultAsync(i => i.ItemId.Equals(itemId));

            var category = await _context.Categories.FirstOrDefaultAsync(i => i.CategoryId.Equals(categoryId));

            await _context.ItemCategories.AddAsync(ItemCategory.Create(item, category));
            await _context.SaveChangesAsync();

            return _mapper.Map<ItemView>(result);
        }
    }
}