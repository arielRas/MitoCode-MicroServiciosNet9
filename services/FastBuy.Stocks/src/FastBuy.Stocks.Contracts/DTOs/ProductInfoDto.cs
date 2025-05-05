namespace FastBuy.Stocks.Contracts.DTOs
{
    public record ProductInfoDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
