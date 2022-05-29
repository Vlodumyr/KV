using InventoryСontrol.Storage.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryСontrol.Api.Infrastructure
{
    public static class DbConfigurationExtensions
    {
        public static IServiceCollection AddDbContextsCustom(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("DefaultConnection");

            services
                .AddDbContext<InventoryСontrolContext>(options => options.UseSqlServer(connection,
                    sqlOptions => sqlOptions.MigrationsAssembly(typeof(InventoryСontrolContext).Assembly.FullName)));

            return services;
        }
    }
}