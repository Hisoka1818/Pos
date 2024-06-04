using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Data;
using Pos.Web.Data.Seeders;
using Pos.Web.Services;
using static Pos.Web.Services.ICustomerService;
using static Pos.Web.Services.ISalesService;
using static Pos.Web.Services.ICategoriesService;
using static Pos.Web.Services.IProductService;
using Microsoft.AspNetCore.Identity;
using Pos.Web.Data.Entities;
using Pos.Web.Helpers;
using PrivatePos.Web.Services;


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

            builder.Services.AddHttpContextAccessor();

            // Services
            AddServices(builder);

            // Identity and Access Managnet
            AddIAM(builder);

            // Toast
            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });

            return builder;
        }

        private static void AddIAM(WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<User, IdentityRole>(x =>
            {
                x.User.RequireUniqueEmail = true;
                x.Password.RequireDigit = false;
                x.Password.RequiredUniqueChars = 0;
                x.Password.RequireLowercase = false;
                x.Password.RequireUppercase = false;
                x.Password.RequireNonAlphanumeric = false;
                x.Password.RequiredLength = 4;
            })
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "Auth";
                options.LoginPath = "/Account/Login"; // Ruta de inicio de sesi�n
                options.AccessDeniedPath = "/Account/NotAuthorized"; // Ruta de acceso denegado
            });

            builder.Services.AddAuthorization();
        }

        private static void AddServices(this WebApplicationBuilder builder)
        {
            // Services
            
            builder.Services.AddScoped<IRolesService, RolesService>();
            builder.Services.AddScoped<ISalesService, SalesService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ICategoriesService, CategoriesService>();
            builder.Services.AddScoped<IProductService, ProductsService>();
            builder.Services.AddTransient<SeedDb>();
            builder.Services.AddScoped<IUsersService, UsersService>();

            //Helper
            builder.Services.AddScoped<IConverterHelper, ConverterHelper>();
        }

        /*public static WebApplication AddCustomConfiguration(this WebApplication app)
        {
            app.UseNotyf();

            SeedData(app);

            return app;
        }

        private static void SeedData(WebApplication app)
        {
            IServiceScopeFactory scopedFactory = app.Services.GetService<IServiceScopeFactory>();

            using (IServiceScope scope = scopedFactory!.CreateScope())
            {
                SeedDb service = scope.ServiceProvider.GetService<SeedDb>();
                service!.SeedAsync().Wait();
            }
        }
        */
    }
}