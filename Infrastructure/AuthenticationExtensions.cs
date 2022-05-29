using InventoryСontrol.Api.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryСontrol.Api.Infrastructure
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddAuthenticationCustom(this IServiceCollection services)
        {
            services
                .AddAuthorizationCore(options =>
                {
                    options.DefaultPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();

                    options.AddPolicy(Policies.User,
                        policy => policy.RequireAuthenticatedUser());
                    options.AddPolicy(Policies.Admin,
                        policy => policy.RequireAuthenticatedUser());
                    options.AddPolicy(Policies.Manager,
                        policy => policy.RequireAuthenticatedUser());
                    options.AddPolicy(Policies.AdminOrManager,
                        policy => policy.RequireRole(Policies.Manager, Policies.Admin));
                });

            return services;
        }
    }
}