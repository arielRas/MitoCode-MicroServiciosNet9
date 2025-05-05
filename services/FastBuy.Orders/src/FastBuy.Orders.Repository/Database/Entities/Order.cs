using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastBuy.Orders.Repository.Database.Entities;

[Table("ORDER")]
public partial class Order
{
    [Key]
    [Column("ORDER_ID")]
    public Guid OrderId { get; set; }

    [Column("CREATE_AT")]
    public DateTime CreateAt { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItem { get; set; } = new List<OrderItem>();

    [InverseProperty("Order")]
    public virtual OrderState? OrderState { get; set; }
}
