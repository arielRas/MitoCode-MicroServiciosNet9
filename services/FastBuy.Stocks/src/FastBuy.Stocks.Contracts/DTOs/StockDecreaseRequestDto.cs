namespace FastBuy.Stocks.Contracts.DTOs
{
    public record StockDecreaseRequestDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
