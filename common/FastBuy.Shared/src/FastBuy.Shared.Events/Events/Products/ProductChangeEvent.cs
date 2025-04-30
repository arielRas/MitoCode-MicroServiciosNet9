namespace FastBuy.Shared.Events.Events.Products
{
    public record ProductChangeEvent
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public required string Description { get; set; }
    }
}
