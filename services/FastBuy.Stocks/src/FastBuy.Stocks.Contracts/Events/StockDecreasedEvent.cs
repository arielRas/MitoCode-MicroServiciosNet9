namespace FastBuy.Stocks.Contracts.Events
{
    public record StockDecreasedEvent
    {
        public Guid CorrelationId { get; set; }
    }
}
