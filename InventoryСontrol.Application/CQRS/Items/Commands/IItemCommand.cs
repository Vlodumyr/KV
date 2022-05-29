using System;
using System.Threading.Tasks;
using InventoryСontrol.Application.CQRS.Items.Views;

namespace InventoryСontrol.Application.CQRS.Items.Commands
{
    public interface IItemCommand
    {
        public Task<ItemView> UpdateAsync(
            Guid itemId,
            string name,
            int? amount,
            int? coast);

        public Task<ItemView> AddAsync(
            string name,
            int amount,
            int cost);

        public Task BuyAsync(
            Guid itemId,
            int amount);

        public Task PreOrderAsync(
            string userId,
            Guid itemId,
            int amount);

        public Task<ItemView> AddCategoryToItemAsync(
            Guid itemId,
            Guid categoryId);
    }
}