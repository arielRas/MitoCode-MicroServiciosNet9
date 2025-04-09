using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace FastBuy.Shared.Library.Security
{
    public static class SecurityExtensions
    {
        public static AuthenticationBuilder AddJwtBearerAuthentication(this IServiceCollection services)
        {
            return services.ConfigureOptions<ConfigureJwtBearerToken>()
                           .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                           .AddJwtBearer();
        }
    }
}
