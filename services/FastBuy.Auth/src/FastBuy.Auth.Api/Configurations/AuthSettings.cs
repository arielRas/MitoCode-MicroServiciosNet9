namespace FastBuy.Auth.Api.Configurations
{
    public class AuthSettings
    {
        public required string AdminUserEmail { get; init; }
        public required string AdminUserPassword { get; init; }
        public required string Name { get; init; }
        public required string LastName { get; init; }
    }
}