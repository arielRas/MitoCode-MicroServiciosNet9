using FastBuy.Orders.Repository.Repositories.Abstractions;
using FastBuy.Orders.Services.Mappers;
using FastBuy.Shared.Events.Events.Products;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FastBuy.Orders.Services.Consumers
{
    public class ProductChangeConsumer : IConsumer<ProductChangeEvent>
    {
        private readonly IProductRepository _repository;

        private readonly ILogger<ProductChangeConsumer> _logger;

        public ProductChangeConsumer(IProductRepository repository, ILogger<ProductChangeConsumer> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProductChangeEvent> context)
        {
            try
            {
                var message = context.Message;

                var product = await _repository.GetByIdAsync(message.Id);

                if (product is null)
                {
                    await _repository.CreateAsync(message.ToEntity());

                    _logger.LogInformation($"[ASYNC-EVENT] - The product has been created with id {message.Id}");
                }
                else
                {
                    await _repository.UpdateAsync(message.Id, message.ToEntity());

                    _logger.LogInformation($"[ASYNC-EVENT] - The product has been updated with id {message.Id}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[ASYNC-EVENT] - An unexpected error has occurred - {ex.Message}");
            }
        }
    }
}
