namespace FastBuy.Stocks.Contracts.Events
{
    public record IncreasedStockEvent
    {
        public Guid CorrelationId { get; set; }
    }
}
