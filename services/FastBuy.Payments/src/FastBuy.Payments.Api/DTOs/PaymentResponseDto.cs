namespace FastBuy.Payments.Api.DTOs
{
    public record PaymentResponseDto
    {
        public Guid PaymentId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public required string Status { get; set; }
    }
}
