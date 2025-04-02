using FastBuy.Products.Contracts.DTOs;
using FastBuy.Products.Contracts.Events;
using FastBuy.Products.Repositories.Abstractions;
using FastBuy.Products.Services.Abstractions;
using FastBuy.Products.Services.Mappers;
using MassTransit;

namespace FastBuy.Products.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IPublishEndpoint _publisher;

        public ProductService(IProductRepository repository, IPublishEndpoint publisher)
        {
            _repository = repository;
            _publisher = publisher;
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

        public async Task<ProductResponseDto> CreateAsync(ProductRequestDto productDto)
        {
            var newProduct = await _repository.CreateAsync(productDto.ToEntity());

            await _publisher.Publish(newProduct.ToChangeEvent());

            return newProduct.ToDto();
        }

        public async Task UpdateAsync(Guid id, ProductRequestDto productDto)
        {
            var product = productDto.ToEntity();

            product.Id = id;

            await _repository.UpdateAsync(id, product);

            await _publisher.Publish(product.ToChangeEvent());
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);

            await _publisher.Publish(new ProductDeletedEvent(id));
        }
    }
}
