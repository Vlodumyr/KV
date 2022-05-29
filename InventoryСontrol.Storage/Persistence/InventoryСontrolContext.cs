using InventoryСontrol.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryСontrol.Storage.Persistence
{
    public class InventoryСontrolContext : IdentityDbContext<User>
    {
        public InventoryСontrolContext(
            DbContextOptions<InventoryСontrolContext> options)
            : base(options)
        {
           // Database.EnsureDeleted();
           Database.EnsureCreated();
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<PreOrder> PreOrders { get; set; }
    }
}