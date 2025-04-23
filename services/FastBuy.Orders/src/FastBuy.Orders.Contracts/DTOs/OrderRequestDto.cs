namespace FastBuy.Orders.Contracts.DTOs
{
    public record OrderRequestDto
    {  
        public required List<OrderItemRequestDto> OrderItems { get; set; }

    }
}
