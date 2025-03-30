using System.ComponentModel.DataAnnotations;

namespace FastBuy.Products.Contracts.Events
{
    public record ProductDeletedEvent
    {
        public ProductDeletedEvent(Guid id) => Id = id;

        [Required]
        public Guid Id { get; }
    }
}
