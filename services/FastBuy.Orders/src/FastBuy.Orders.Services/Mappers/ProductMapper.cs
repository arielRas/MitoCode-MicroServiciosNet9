using FastBuy.Orders.Repository.Database.Entities;
using FastBuy.Shared.Events.Events.Products;

namespace FastBuy.Orders.Services.Mappers
{
    internal static class ProductMapper
    {
        //ProductChangeEvent => Product
        public static Product ToEntity(this ProductChangeEvent productEvent)
        {
            return new Product
            {
                ProductId = productEvent.Id,
                Name = productEvent.Name,
                Description = productEvent.Description,
                Price = productEvent.Price,
                IsActive = true
            };
        }
    }
}
