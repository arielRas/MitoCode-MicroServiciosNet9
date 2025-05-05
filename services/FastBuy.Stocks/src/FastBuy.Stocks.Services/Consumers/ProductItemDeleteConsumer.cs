using DnsClient.Internal;
using FastBuy.Shared.Events.Events.Products;
using FastBuy.Shared.Library.Repository.Abstractions;
using FastBuy.Stocks.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace FastBuy.Stocks.Services.Consumers
{
    public class ProductItemDeleteConsumer : IConsumer<ProductDeletedEvent>
    {
        private readonly IRepository<ProductItem> _productRepository;
        private readonly IRepository<StockItem> _stockRepository;
        private readonly ILogger<ProductItemDeleteConsumer> _logger;

        public ProductItemDeleteConsumer(IRepository<ProductItem> repository, IRepository<StockItem> stockRepository, ILogger<ProductItemDeleteConsumer> logger)
        {
            _productRepository = repository;
            _stockRepository = stockRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
        {
            try
            {
                var message = context.Message;

                Expression<Func<ProductItem, bool>> productFilter = x => x.Id == message.Id;

                if (!await _productRepository.ExistAsync(productFilter)) return;

                await _productRepository.DeleteAsync(message.Id);

                _logger.LogInformation($"[ASYNC-EVENT] - The productItem has been deleted with id {message.Id}");

                Expression<Func<StockItem, bool>> stockFilter = x => x.ProductId == message.Id;

                var stockItem = await _stockRepository.GetAsync(stockFilter);

                await _stockRepository.DeleteAsync(stockItem.Id);

                _logger.LogInformation($"[ASYNC-EVENT] - The stockItem has been deleted with id {message.Id}");
            }
            catch (Exception ex) 
            {
                _logger.LogError($"[ASYNC-EVENT] - An unexpected error has occurred - {ex.Message}");
            }
        }
    }
}
