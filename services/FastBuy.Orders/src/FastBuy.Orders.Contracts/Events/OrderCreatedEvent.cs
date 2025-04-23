using FastBuy.Orders.Contracts.DTOs;

namespace FastBuy.Orders.Contracts.Events
{
    public record OrderCreatedEvent
    {        
        public Guid CorrelationId { get; set; }
        public required IEnumerable<OrderItemRequestDto> OrderItems { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
