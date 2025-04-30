namespace FastBuy.Stocks.Contracts.Events
{
    public record StockIncreasedEvent
    {
        public Guid CorrelationId { get; set; }
    }
}
