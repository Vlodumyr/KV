using System;
using System.Reflection;
using InventoryСontrol.Api.Infrastructure;
using InventoryСontrol.Api.Infrastructure.Helpers;
using InventoryСontrol.Api.Infrastructure.Seed;
using InventoryСontrol.Application.CQRS.Categories.Commands;
using InventoryСontrol.Application.CQRS.Categories.Queries;
using InventoryСontrol.Application.CQRS.Items.Commands;
using InventoryСontrol.Application.CQRS.Items.Queries;
using InventoryСontrol.Application.CQRS.UserAccounts.Role.Commands;
using InventoryСontrol.Application.CQRS.UserAccounts.Users.Queries;
using InventoryСontrol.Application.Mapper;
using InventoryСontrol.Domain;
using InventoryСontrol.Storage.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InventoryСontrol.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextsCustom(Configuration);
            services.AddControllers();
            services
                .AddIdentity<User, IdentityRole>(ops =>
                {
                    ops.SignIn.RequireConfirmedEmail = false;
                    ops.User.RequireUniqueEmail = true;
                    ops.Password.RequireDigit = false;
                    ops.Password.RequireUppercase = false;
                    ops.Password.RequireNonAlphanumeric = false;
                    ops.Password.RequiredLength = 5;
                    ops.SignIn.RequireConfirmedAccount = false;
                })
                .AddEntityFrameworkStores<InventoryСontrolContext>();

            services.AddScoped<ISeedService, SeedService>();
            services.AddTransient<IItemQuery, ItemQuery>();
            services.AddTransient<IItemCommand, ItemCommand>();
            services.AddTransient<IUserRoleCommand, UserRoleCommand>();
            services.AddTransient<IUserAccountQuery, UserAccountQuery>();
            services.AddTransient<ICategoryCommand, CategoryCommand>();
            services.AddTransient<ICategoryQuery, CategoryQuery>();

            services.AddAuthenticationCustom();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(RegisterViews).GetTypeInfo().Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var errorResponse = ErrorHelper.CreateErrorResponse(contextFeature.Error, env.IsDevelopment());

                        context.Response.StatusCode = (int)errorResponse.StatusCode;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(errorResponse.ToString());
                    }
                });
            });
        }
    }
}