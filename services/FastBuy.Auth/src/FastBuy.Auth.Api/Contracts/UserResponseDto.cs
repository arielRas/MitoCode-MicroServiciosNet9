using System.ComponentModel.DataAnnotations;

namespace FastBuy.Auth.Api.Contracts
{
    public record UserResponseDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }

        [EmailAddress]
        public required string Email { get; set; }
    }
}
