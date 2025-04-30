using FastBuy.Products.Contracts.DTOs;
using FastBuy.Products.Entities;
using FastBuy.Shared.Events.Events.Products;

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

        public static ProductChangeEvent ToChangeEvent(this Product entity)
        {
            return new ProductChangeEvent
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                Description = entity.Description
            };
        }

        public static Product ToEntity(this ProductRequestDto dto)
        {
            return new Product
            {
                Name = dto.Name!,
                Description = dto.Description!,
                Price = dto.Price,
                CreatedAt = DateTimeOffset.UtcNow
            };
        }
    }
}
