using FastBuy.Products.Contracts.DTOs;

namespace FastBuy.Products.Services.Abstractions
{
    public interface IProductService
    {
        Task<ProductResponseDto> GetByIdAsync(Guid id);
        Task<IEnumerable<ProductResponseDto>> GetAllAsync();
        Task<ProductResponseDto> CreateAsync(ProductRequestDto product);
        Task UpdateAsync(Guid id, ProductRequestDto product);
        Task DeleteAsync(Guid id);
    }
}
