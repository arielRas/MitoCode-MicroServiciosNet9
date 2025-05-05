using System.ComponentModel.DataAnnotations;

namespace FastBuy.Orders.Contracts.DTOs
{
    public class OrderItemRequestDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
