using FastBuy.Products.Contracts.DTOs;
using FastBuy.Products.Contracts.Events;

namespace FastBuy.Products.Api.Mappers
{
    internal static class ProductMapper
    {
        public static ProductChangeEvent ToChangeEvent(this ProductResponseDto dto)
        {
            return new ProductChangeEvent
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description
            };
        }

        public static ProductChangeEvent ToChangeEvent(this ProductRequestDto dto, Guid id)
        {
            return new ProductChangeEvent
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description
            };
        }
    }
}
