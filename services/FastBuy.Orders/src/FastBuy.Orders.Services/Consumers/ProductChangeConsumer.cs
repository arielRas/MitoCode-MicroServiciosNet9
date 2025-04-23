using FastBuy.Orders.Repository.Repositories.Abstractions;
using FastBuy.Orders.Services.Mappers;
using FastBuy.Products.Contracts.Events;
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
            var message = context.Message;

            var product = await _repository.GetByIdAsync(message.Id);

            if(product is null)
            {
                await _repository.CreateAsync(message.ToEntity());
            }
            else
            {
                await _repository.UpdateAsync(message.ToEntity());
            }            
        }
    }
}
