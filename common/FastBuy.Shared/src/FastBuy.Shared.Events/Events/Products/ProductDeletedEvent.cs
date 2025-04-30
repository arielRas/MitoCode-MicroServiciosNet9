namespace FastBuy.Shared.Events.Events.Products
{
    public record ProductDeletedEvent
    {
        public Guid Id { get; set; }
    }
}
