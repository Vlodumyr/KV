using System.Linq;
using System.Threading.Tasks;
using InventoryСontrol.Api.Persistence;
using InventoryСontrol.Domain;
using InventoryСontrol.Storage.Persistence;
using Microsoft.AspNetCore.Identity;

namespace InventoryСontrol.Api.Infrastructure.Seed
{
    internal sealed class SeedService : ISeedService
    {
        private readonly InventoryСontrolContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public SeedService(
            InventoryСontrolContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedRolesAsync()
        {
            await _roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await _roleManager.CreateAsync(new IdentityRole(Roles.Manager.ToString()));
            await _roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
        }

        public async Task SeedAdminAndManagerAsync()
        {
            var defaultAdmin = new User
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (_userManager.Users.All(u => u.Id != defaultAdmin.Id))
            {
                var user = await _userManager.FindByEmailAsync(defaultAdmin.Email);
                if (user == null)
                {
                    await _userManager.CreateAsync(defaultAdmin, "admin");
                    await _userManager.AddToRoleAsync(defaultAdmin, Roles.Admin.ToString());
                    await _userManager.AddToRoleAsync(defaultAdmin, Roles.Manager.ToString());
                    await _userManager.AddToRoleAsync(defaultAdmin, Roles.User.ToString());
                }
            }

            var defaultManager = new User
            {
                UserName = "manager",
                Email = "manager@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (_userManager.Users.All(u => u.Id != defaultManager.Id))
            {
                var user = await _userManager.FindByEmailAsync(defaultManager.Email);
                if (user == null)
                {
                    await _userManager.CreateAsync(defaultManager, "manager");
                    await _userManager.AddToRoleAsync(defaultManager, Roles.Manager.ToString());
                    await _userManager.AddToRoleAsync(defaultManager, Roles.User.ToString());
                }
            }
        }

        public async Task SeedCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.AddRange(
                    Category.Create("Cтройматериалы"),
                    Category.Create("Телефоны"),
                    Category.Create("Ноутбуки")
                );

                await _context.SaveChangesAsync();
            }
        }

        public async Task SeedItemsAsync()
        {
            if (!_context.Items.Any())
            {
                _context.Items.AddRange(
                    Item.Create("Брус", 100, 5),
                    Item.Create("Доска", 50, 10),
                    Item.Create("Asus G500", 50000, 31),
                    Item.Create("Lenovo A100", 25000, 10),
                    Item.Create("Iphone 6+", 10000, 100),
                    Item.Create("Revo Pnone", 5000, 59)
                );

                await _context.SaveChangesAsync();
            }
        }

        public async Task SeedAddCategoryToItemsAsync()
        {
            if (!_context.ItemCategories.Any())
            {
                var itemBrus = _context.Items
                    .FirstOrDefault(i => i.Name.Equals("Брус"));
                var itemDoska = _context.Items
                    .FirstOrDefault(i => i.Name.Equals("Доска"));
                var itemAsus = _context.Items
                    .FirstOrDefault(i => i.Name.Equals("Asus G500"));
                var itemLenovo = _context.Items
                    .FirstOrDefault(i => i.Name.Equals("Lenovo A100"));
                var itemIphone = _context.Items
                    .FirstOrDefault(i => i.Name.Equals("Iphone 6+"));
                var itemRevo = _context.Items
                    .FirstOrDefault(i => i.Name.Equals("Revo Pnone"));

                var categoryBild = _context.Categories
                    .FirstOrDefault(i => i.Name.Equals("Cтройматериалы"));
                var categoryPhone = _context.Categories
                    .FirstOrDefault(i => i.Name.Equals("Телефоны"));
                var categoryNotebook = _context.Categories
                    .FirstOrDefault(i => i.Name.Equals("Ноутбуки"));


                await _context.ItemCategories.AddRangeAsync(
                    ItemCategory.Create(itemBrus, categoryBild),
                    ItemCategory.Create(itemDoska, categoryBild),
                    ItemCategory.Create(itemAsus, categoryNotebook),
                    ItemCategory.Create(itemLenovo, categoryNotebook),
                    ItemCategory.Create(itemIphone, categoryPhone),
                    ItemCategory.Create(itemRevo, categoryPhone));

                await _context.SaveChangesAsync();
            }
        }
    }
}