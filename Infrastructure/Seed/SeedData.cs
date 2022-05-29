using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryСontrol.Api.Infrastructure.Seed
{
    public class SeedData
    {
        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var seedService = scope.ServiceProvider.GetRequiredService<ISeedService>();
            await seedService.SeedRolesAsync();
            await seedService.SeedAdminAndManagerAsync();
            await seedService.SeedCategoriesAsync();
            await seedService.SeedItemsAsync();
            await seedService.SeedAddCategoryToItemsAsync();
        }
    }
}