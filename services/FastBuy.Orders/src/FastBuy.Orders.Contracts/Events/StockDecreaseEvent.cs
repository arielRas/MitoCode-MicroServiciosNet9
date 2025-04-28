using FastBuy.Orders.Contracts.DTOs;

namespace FastBuy.Orders.Contracts.Events
{
    public class StockDecreaseEvent
    {        
        public Guid CorrelationId { get; set; }
        public required IEnumerable<OrderItemRequestDto> Items { get; set; }
    }
}
