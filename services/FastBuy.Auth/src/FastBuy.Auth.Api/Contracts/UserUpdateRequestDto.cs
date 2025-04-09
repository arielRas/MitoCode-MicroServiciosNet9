namespace FastBuy.Auth.Api.Contracts
{
    public record UserUpdateRequestDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Lastname { get; set; }
    }
}
