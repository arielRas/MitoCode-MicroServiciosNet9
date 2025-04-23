namespace FastBuy.Orders.Contracts.Events
{
    public class StockFailedDecreaseEvent
    {
        public Guid CorrelationId { get; set; }
        public Guid ProductId { get; set; }
        public required string Reason { get; set; }
    }
}
