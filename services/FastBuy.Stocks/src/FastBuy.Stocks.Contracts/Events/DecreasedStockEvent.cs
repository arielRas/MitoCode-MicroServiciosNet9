namespace FastBuy.Stocks.Contracts.Events
{
    public record DecreasedStockEvent
    {
        public Guid CorrelationId { get; set; }
    }
}
