using FastBuy.Products.Contracts.DTOs;
using FastBuy.Products.Repositories.Abstractions;
using FastBuy.Products.Services.Abstractions;
using FastBuy.Products.Services.Mappers;

namespace FastBuy.Products.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
           _repository = repository;
        }

        public async Task<ProductResponseDto> GetByIdAsync(Guid id)
        {
            var product = await _repository.GetByIdAsync(id);

            return product.ToDto();
        }
        public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();

            return products.Select(p => p.ToDto());
        }

        public async Task<ProductResponseDto> CreateAsync(ProductRequestDto product)
        {
            var newProduct = await _repository.CreateAsync(product.ToEntity());

            return newProduct.ToDto();
        }

        public async Task UpdateAsync(Guid id, ProductRequestDto product)
        {
            await _repository.UpdateAsync(id, product.ToEntity());
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
