using System.ComponentModel.DataAnnotations;

namespace FastBuy.Products.Contracts.Events
{
    public record ProductChangeEvent
    {
        [Required]
        public required Guid Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public required string Description { get; set; }
    }
}
