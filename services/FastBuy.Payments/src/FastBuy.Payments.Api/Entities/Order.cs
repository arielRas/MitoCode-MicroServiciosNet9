using System.ComponentModel.DataAnnotations;

namespace FastBuy.Payments.Api.Entities
{
    public class Order
    {
        [Key]
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public Payment? Payment { get; set; }
    }
}
