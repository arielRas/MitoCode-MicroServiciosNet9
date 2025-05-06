using FastBuy.Products.Contracts.DTOs;
using FastBuy.Products.Entities;
using FastBuy.Products.Services.Abstractions;
using FastBuy.Products.Services.Mappers;
using FastBuy.Shared.Events.Events.Products;
using FastBuy.Shared.Library.Repository.Abstractions;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FastBuy.Products.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _repository;
        private readonly ILogger  _logger;
        private readonly IPublishEndpoint _publisher;

        public ProductService(IRepository<Product> repository, ILogger<ProductService> logger ,IPublishEndpoint publisher)
        {
            _repository = repository;
            _publisher = publisher;
            _logger = logger;
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
            var newProduct = productDto.ToEntity();

            await _repository.CreateAsync(newProduct);

            await PublishEvent(newProduct.ToChangeEvent());

            return newProduct.ToDto();
        }

        public async Task UpdateAsync(Guid id, ProductRequestDto productDto)
        {
            var product = productDto.ToEntity();

            product.Id = id;

            await _repository.UpdateAsync(id, product);

            await PublishEvent(product.ToChangeEvent());
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);

            await PublishEvent(new ProductDeletedEvent { Id = id});
        }        

        private async Task PublishEvent<T>(T eventEntity) where T : class
        {
            await _publisher.Publish(eventEntity);

            _logger.LogInformation($"[ASYNC-EVENT] - Send {eventEntity.GetType().Name}");
        }
    }
}
