using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBuy.Orders.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        public required IEnumerable<Product> Items { get; set; }
    }
}
