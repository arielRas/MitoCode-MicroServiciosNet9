namespace FastBuy.Orders.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        public required ICollection<OrderItem> Items { get; set; }

        public DateTimeOffset CreatedAt {  get; set; }
    }
}
