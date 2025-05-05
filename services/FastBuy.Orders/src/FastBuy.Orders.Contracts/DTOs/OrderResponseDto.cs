namespace FastBuy.Orders.Contracts.DTOs
{
    public record OrderResponseDto
    {
        public Guid Id { get; set; }

        public required string State { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public decimal Amount { get; set; }

        public required List<OrderItemResponseDto> OrderItems { get; set; }
    }
}
