namespace FastBuy.Stocks.Contracts.Events
{
    public record IncreaseStockEvent
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public Guid CorrelationId { get; set; }
    }
}
