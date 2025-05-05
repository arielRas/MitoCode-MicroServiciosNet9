using FastBuy.Shared.Library.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FastBuy.Shared.Library.Security
{
    public class ConfigureJwtBearerToken : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly IConfiguration _configuration;

        public ConfigureJwtBearerToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(string? name, JwtBearerOptions options)
        {
            if (name == JwtBearerDefaults.AuthenticationScheme)
            {
                var serviceSettings = _configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()
                    ?? throw new ArgumentNullException($"The {nameof(ServiceSettings)} key has not been configured in the configuration file.");

                options.Authority = serviceSettings.Authority;
                options.Audience = serviceSettings.ServiceName;
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role",
                };
            }
        }

        public void Configure(JwtBearerOptions options)
        {
            Configure(Options.DefaultName, options);
        }
    }
}
