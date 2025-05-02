using System.ComponentModel.DataAnnotations;

namespace FastBuy.Payments.Api.DTOs
{
    public record OrderResponseDto
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public PaymentResponseDto? Payment { get; set; }
    }
}
