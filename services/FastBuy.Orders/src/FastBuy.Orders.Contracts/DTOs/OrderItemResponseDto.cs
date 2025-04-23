namespace FastBuy.Orders.Contracts.DTOs
{
    public record OrderItemResponseDto
    {
        public Guid ProductId { get; set; }
        public required string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
