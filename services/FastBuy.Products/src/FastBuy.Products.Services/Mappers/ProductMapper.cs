using FastBuy.Products.Contracts;
using FastBuy.Products.Entities;

namespace FastBuy.Products.Services.Mappers
{
    internal static class ProductMapper
    {
        public static ProductResponseDto ToDto(this Product entity)
        {
            return new ProductResponseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                CreatedAt = entity.CreatedAt
            };
        }

        public static Product ToEntity(this ProductRequestDto dto)
        {
            return new Product
            {                
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                CreatedAt = DateTimeOffset.UtcNow
            };
        }
    }
}
