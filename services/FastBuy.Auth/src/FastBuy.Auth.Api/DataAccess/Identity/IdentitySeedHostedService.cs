using FastBuy.Auth.Api.Configurations;
using FastBuy.Auth.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace FastBuy.Auth.Api.DataAccess.Identity
{
    public class IdentitySeedHostedService : IHostedService
    {
        public IServiceScopeFactory _serviceScopeFactory;
        public IOptions<AuthSettings> _options;
        public ILogger<IdentitySeedHostedService> _logger;

        public IdentitySeedHostedService(IServiceScopeFactory serviceScopeFactory, IOptions<AuthSettings> options, ILogger<IdentitySeedHostedService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _options = options;
            _logger = logger;
        }        

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var serviceProvider = scope.ServiceProvider;

            try
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

                var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

                await EnsureRolesExistsAsync(roleManager);

                await EnsureAdminUserExistsAsync(userManager);

                _logger.LogInformation("Identity seeding completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding identity data.");               
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task EnsureRolesExistsAsync(RoleManager<AppRole> roleManager)
        {
            var existingRoles = new [] { Role.Admin, Role.Customer };

            foreach(var role in existingRoles)
            {
                if(!await roleManager.RoleExistsAsync(role).ConfigureAwait(false))
                {
                    var newAppRole = new AppRole { Name = role };

                    await roleManager.CreateAsync(newAppRole).ConfigureAwait(false);
                }
            }
        }

        private async Task EnsureAdminUserExistsAsync(UserManager<AppUser> userManager)
        {
            var adminEmail = _options.Value.AdminUserEmail;            

            var adminUser = await userManager.FindByEmailAsync(adminEmail).ConfigureAwait(false);

            if (adminUser == null)
            {
                _logger.LogInformation("Creating admin user '{AdminEmail}'...", adminEmail);

                adminUser = new AppUser
                {
                    UserName = _options.Value.AdminUserName,
                    Email = adminEmail,
                    Name = _options.Value.Name,
                    LastName = _options.Value.LastName
                };

                var adminPassword = _options.Value.AdminUserPassword;

                var result = await userManager.CreateAsync(adminUser, adminPassword).ConfigureAwait(false);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, Role.Admin).ConfigureAwait(false);

                    _logger.LogInformation($"Admin user '{adminEmail}' created successfully.");
                }
                else
                {
                    _logger.LogError($"Failed to create admin user '{adminEmail}': {string.Join(", ", result.Errors)}");
                }
            }
            else
            {
                _logger.LogInformation($"Admin user '{adminEmail}' already exists.");
            }
        }
    }
}
