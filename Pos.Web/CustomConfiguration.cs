using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Data;
using Pos.Web.Services;
using static Pos.Web.Services.IProductsService;


namespace Pos.Web
{
    public static class CustomConfiguration
    {
        public static WebApplicationBuilder AddCustomBuilderConfiguration(this WebApplicationBuilder builder)
        {
            // Data Context
            builder.Services.AddDbContext<DataContext>(conf =>
            {
                conf.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
            });

            // Services
            AddServices(builder);

            // Toast
            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });

            return builder;
        }

        private static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IProductsService, ProductsService>();
            
        }

        public static WebApplication AddCustomConfiguration(this WebApplication app)
        {
            app.UseNotyf();

            return app;
        }

    }
}