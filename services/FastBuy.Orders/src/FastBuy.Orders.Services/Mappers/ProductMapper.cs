using FastBuy.Orders.Entities;
using FastBuy.Products.Contracts.Events;

namespace FastBuy.Orders.Services.Mappers
{
    internal static class ProductMapper
    {
        public static Product ToEntity(this ProductChangeEvent productEvent)
        {
            return new Product
            {
                Id = productEvent.Id,
                Name = productEvent.Name,
                Description = productEvent.Description,
                Price = productEvent.Price
            };
        }
    }
}
