using System.ComponentModel.DataAnnotations;

namespace FastBuy.Products.Entities;

public class Product
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Description { get; set; }

    [Required]
    public decimal Price { get; set; }
    
    [Required]
    public DateTimeOffset CreatedAt { get; set; }
}
