using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastBuy.Orders.Repository.Database.Entities;

[PrimaryKey("OrderId", "ProductId")]
[Table("ORDER_ITEM")]
public partial class OrderItem
{
    [Key]
    [Column("ORDER_ID")]
    public Guid OrderId { get; set; }

    [Key]
    [Column("PRODUCT_ID")]
    public Guid ProductId { get; set; }

    [Column("QUANTITY")]
    public int Quantity { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderItem")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("OrderItem")]
    public virtual Product Product { get; set; } = null!;
}
