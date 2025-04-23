using System.ComponentModel.DataAnnotations;

namespace FastBuy.Orders.Entities
{
    public class Order : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public required ICollection<OrderItem> Items { get; set; }

        [Required]
        public required string State { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; }

        [Required]
        public DateTimeOffset LastUpdate { get; set; }
    }
}
