using System.ComponentModel.DataAnnotations;

namespace FastBuy.Payments.Api.DTOs
{
    public record PaymentRequestDto
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
