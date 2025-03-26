namespace FastBuy.Stocks.Contracts
{
    public record StockDecreaseRequestDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
