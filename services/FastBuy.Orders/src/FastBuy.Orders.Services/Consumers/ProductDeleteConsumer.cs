using FastBuy.Orders.Repository.Repositories.Abstractions;
using FastBuy.Shared.Events.Events.Products;
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
            try
            {
                var message = context.Message;

                await _repository.SetProductActiveAsync(message.Id, false);

                _logger.LogInformation($"[ASYNC-EVENT] - Product with id {message.Id} has become inactive");
            }
            catch(KeyNotFoundException ex)
            {
                _logger.LogError($"[ASYNC-EVENT] - Non-existent product to delete - {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[ASYNC-EVENT] - An unexpected error has occurred - {ex.Message}");
            }
        }
    }
}
