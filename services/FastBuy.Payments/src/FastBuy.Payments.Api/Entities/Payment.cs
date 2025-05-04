using System.ComponentModel.DataAnnotations;

namespace FastBuy.Payments.Api.Entities
{
    public class Payment
    {
        [Key]
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public DateTimeOffset OrderDate { get; set; }

        public DateTimeOffset? PaymentDate { get; set; }

        [Required]
        public required string Status { get; set; }

        public Order? Order { get; set; }
    }
}
