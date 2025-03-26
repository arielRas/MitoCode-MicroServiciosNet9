namespace FastBuy.Stocks.Contracts
{
    public record StockResponseDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTimeOffset LastUpdate { get; set; }
    }
}
