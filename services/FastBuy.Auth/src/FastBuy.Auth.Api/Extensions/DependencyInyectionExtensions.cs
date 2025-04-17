using FastBuy.Auth.Api.Configurations;
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

            var identityServiceSettings = configuration.GetSection(nameof(IdentityServerSettings)).Get<IdentityServerSettings>()
                ?? throw new ArgumentNullException("IdentityServerSettings key is not configured");


            //Configuration option pattern classes
            services.Configure<AuthSettings>(configuration.GetSection(nameof(AuthSettings)));


            //DbContext Register
            services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(connectionString));


            //IdentityEF register
            services.AddIdentity<AppUser, AppRole>()
                    .AddEntityFrameworkStores<AuthDbContext>()
                    .AddDefaultTokenProviders()
                    .AddDefaultUI();


            //Identity Server register and configure
            services.AddIdentityServer(options =>
            {
                options.Events.RaiseSuccessEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseErrorEvents = true;
            })
            .AddAspNetIdentity<AppUser>()
            .AddInMemoryApiScopes(identityServiceSettings.ApiScopes)
            .AddInMemoryApiResources(identityServiceSettings.ApiResources)
            .AddInMemoryClients(identityServiceSettings.Clients)
            .AddInMemoryIdentityResources(identityServiceSettings.IdentityResources)
            .AddDeveloperSigningCredential();


            //Authentication for this api
            services.AddLocalApiAuthentication();


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
