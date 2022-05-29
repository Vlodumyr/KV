using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryСontrol.Domain;
using InventoryСontrol.Storage.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryСontrol.Tests
{
    public class InventoryСontrolContextFixture : IDisposable
    {
        public readonly InventoryСontrolContext context;

        public InventoryСontrolContextFixture()
        {
            var time = DateTime.UtcNow.Millisecond.ToString();
            var options = new DbContextOptionsBuilder<InventoryСontrolContext>()
                .UseInMemoryDatabase($"InventoryСontrol{time}")
                .Options;

            context = new InventoryСontrolContext(options);
        }

        public void Dispose()
        {
            context?.Dispose();
        }

        public async Task InitUsersAsync(IEnumerable<User> users)
        {
            context.Users.AddRange(users);
            await context.SaveChangesAsync();
        }

        public async Task InitItemsAsync(IEnumerable<Item> items)
        {
            context.Items.AddRange(items);
            await context.SaveChangesAsync();
        }

        public async Task InitCategoresAsync(IEnumerable<Category> categores)
        {
            context.Categories.AddRange(categores);
            await context.SaveChangesAsync();
        }

        public async Task InitItemCategoresAsync(IEnumerable<ItemCategory> itemCategory)
        {
            context.ItemCategories.AddRange(itemCategory);
            await context.SaveChangesAsync();
        }
    }
}