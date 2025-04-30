namespace FastBuy.Stocks.Contracts.Events
{
    public record StockFailedDecreaseEvent
    {
        public Guid CorrelationId { get; set; }
        public Guid ProductId { get; set; }
        public required string Reason { get; set; }
    }
}
