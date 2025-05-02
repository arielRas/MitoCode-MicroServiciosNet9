namespace FastBuy.Shared.Events.Saga.Orders
{
    public record OrderCreatedEvent
    {
        public Guid CorrelationId { get; set; }
        public Guid OrderId { get; set; }        
        public DateTime CreatedAt { get; set; }
        public required IEnumerable<OrderItem> Items { get; set; }
        public decimal Amount { get; set; }
    }
}
