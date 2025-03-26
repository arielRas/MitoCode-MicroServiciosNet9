namespace FastBuy.Stocks.Entities
{
    public class StockItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Stock { get; set; }
        public DateTimeOffset LastUpdate { get; set; }
    }
}
