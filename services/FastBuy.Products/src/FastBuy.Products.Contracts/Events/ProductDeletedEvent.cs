using System.ComponentModel.DataAnnotations;

namespace FastBuy.Products.Contracts.Events
{
    public record ProductDeletedEvent(Guid id)
    {
        [Required]
        public Guid Id { get; } = id;
    }
}
