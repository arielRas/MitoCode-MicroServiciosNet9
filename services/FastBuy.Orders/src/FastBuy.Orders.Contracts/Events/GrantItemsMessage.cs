namespace FastBuy.Orders.Contracts.Events
{
    public record GrantItemsMessage
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public Guid CorrelationId { get; set; }
    }
}
