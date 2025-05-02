namespace FastBuy.Orders.Entities
{
    public class OrderItem
    {
        public Guid OrderId { get; set; }        
        public Guid ProductId { get; set; }        
        public int Quantity { get; set; }

        //Navigation properties
        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}
