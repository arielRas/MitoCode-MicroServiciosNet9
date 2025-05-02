using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastBuy.Orders.Repository.Database.Entities;

[Table("PRODUCT")]
public partial class Product
{
    [Key]
    [Column("PRODUCT_ID")]
    public Guid ProductId { get; set; }

    [Column("NAME")]
    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("DESCRIPTION")]
    [Unicode(false)]
    public string Description { get; set; } = null!;

    [Column("PRICE", TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<OrderItem> OrderItem { get; set; } = new List<OrderItem>();
}
