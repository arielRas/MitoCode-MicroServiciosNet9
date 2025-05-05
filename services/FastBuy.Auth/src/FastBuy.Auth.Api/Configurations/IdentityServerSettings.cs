using Duende.IdentityServer.Models;

namespace FastBuy.Auth.Api.Configurations
{
    public class IdentityServerSettings
    {
        public IReadOnlyCollection<ApiScope> ApiScopes { get; set; } = Array.Empty<ApiScope>();
        public IReadOnlyCollection<ApiResource> ApiResources { get; set; } = Array.Empty<ApiResource>();
        public IReadOnlyCollection<Client> Clients { get; set; } = Array.Empty<Client>();
        public IReadOnlyCollection<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
    }
}
