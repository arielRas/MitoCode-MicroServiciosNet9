namespace FastBuy.Orders.Contracts.Events
{
    public record StockIncreaseEvent
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public Guid CorrelationId { get; set; }
    }
}
