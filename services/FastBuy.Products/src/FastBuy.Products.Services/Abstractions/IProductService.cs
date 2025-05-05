using FastBuy.Products.Contracts.DTOs;

namespace FastBuy.Products.Services.Abstractions
{
    public interface IProductService
    {
        Task<ProductResponseDto> GetByIdAsync(Guid id);
        Task<IEnumerable<ProductResponseDto>> GetAllAsync();
        Task<ProductResponseDto> CreateAsync(ProductRequestDto productDto);
        Task UpdateAsync(Guid id, ProductRequestDto productDto);
        Task DeleteAsync(Guid id);
    }
}
