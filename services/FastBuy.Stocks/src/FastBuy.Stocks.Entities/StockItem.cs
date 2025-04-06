using FastBuy.Shared.Library.Repository.Abstractions;

namespace FastBuy.Stocks.Entities
{
    public class StockItem : IBaseEntity
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Stock { get; set; }
        public DateTimeOffset LastUpdate { get; set; }
    }
}
