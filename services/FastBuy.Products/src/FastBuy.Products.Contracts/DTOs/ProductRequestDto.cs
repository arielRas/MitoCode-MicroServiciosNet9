using System.ComponentModel.DataAnnotations;

namespace FastBuy.Products.Contracts.DTOs
{
    public record ProductRequestDto
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
