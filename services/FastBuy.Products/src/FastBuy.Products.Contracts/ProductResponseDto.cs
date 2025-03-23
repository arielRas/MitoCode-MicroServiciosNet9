using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FastBuy.Products.Contracts
{
    public record ProductResponseDto : ProductRequestDto
    {
        [Required]
        public Guid Id { get; set; }

        [JsonPropertyOrder(5)]
        [Required]
        public DateTimeOffset CreatedAt { get; set; }
    }
}
