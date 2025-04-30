namespace FastBuy.Shared.Events.Saga.Orders
{
    public record OrderItem
    {        
        public Guid ProductId { get; set; }
        
        public int Quantity { get; set; }
    }
}
