using FastBuy.Auth.Api.DataAccess;
using FastBuy.Auth.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FastBuy.Auth.Api.Extensions
{
    public static class DependencyInyectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Configuration values
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string is not configured");


            //DbContext Register
            services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(connectionString));

            //IdentityEF register
            services.AddIdentity<AppUser, AppRole>()
                    .AddEntityFrameworkStores<AuthDbContext>()
                    .AddDefaultTokenProviders()
                    .AddDefaultUI();

            //IdentityEF configuration
            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
            });

            return services;
        }

        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            //Controller registration
            services.AddControllers();

            //configure Swagger as an endpoint explorer
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }


    }
}
