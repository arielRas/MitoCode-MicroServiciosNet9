using System.ComponentModel.DataAnnotations;

namespace FastBuy.Products.Contracts.Events
{
    public record ProductChangeEvent
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }
    }
}
