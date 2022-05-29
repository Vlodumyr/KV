using System;
using System.Threading.Tasks;
using InventoryСontrol.Storage.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryСontrol.Application.Extensions
{
    public static class EntityValidator
    {
        public static async Task<bool> TryGetItemAsync(
            this InventoryСontrolContext context,
            Guid itemId)
        {
            var result = await context.Items.FirstOrDefaultAsync(i => i.ItemId.Equals(itemId));
            return result != null;
        }

        public static async Task<bool> TryGetItemAsync(
            this InventoryСontrolContext context,
            string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var result = await context.Items.FirstOrDefaultAsync(i => i.Name.Equals(name));
                return result != null;
            }

            return false;
        }

        public static async Task<bool> TryGetCategoryAsync(
            this InventoryСontrolContext context,
            Guid itemId)
        {
            var result = await context.Categories.FirstOrDefaultAsync(i => i.CategoryId.Equals(itemId));
            return result != null;
        }

        public static async Task<bool> TryGetCategoryAsync(
            this InventoryСontrolContext context,
            string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var result = await context.Categories.FirstOrDefaultAsync(i => i.Name.Equals(name));
                return result != null;
            }

            return false;
        }
    }
}