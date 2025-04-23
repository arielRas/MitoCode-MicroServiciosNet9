using FastBuy.Orders.Repository.Repositories.Abstractions;
using FastBuy.Products.Contracts.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FastBuy.Orders.Services.Consumers
{
    public class ProductDeleteConsumer : IConsumer<ProductDeletedEvent>
    {
        private readonly IProductRepository _repository;

        private readonly ILogger<ProductDeleteConsumer> _logger;

        public ProductDeleteConsumer(IProductRepository repository, ILogger<ProductDeleteConsumer> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
        {
            var message = context.Message;

            var product = await _repository.GetByIdAsync(message.Id);

            if (product is not null)
                await _repository.DeleteAsync(product.Id);

        }
    }
}
