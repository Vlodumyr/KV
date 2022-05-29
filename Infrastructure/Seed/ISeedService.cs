using System.Threading.Tasks;

namespace InventoryСontrol.Api.Infrastructure.Seed
{
    internal interface ISeedService
    {
        public Task SeedRolesAsync();
        public Task SeedAdminAndManagerAsync();
        public Task SeedCategoriesAsync();
        public Task SeedItemsAsync();
        public Task SeedAddCategoryToItemsAsync();
    }
}