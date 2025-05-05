using DnsClient.Internal;
using FastBuy.Shared.Events.Events.Products;
using FastBuy.Shared.Library.Repository.Abstractions;
using FastBuy.Stocks.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace FastBuy.Stocks.Services.Consumers
{
    public class ProductItemChangeConsumer : IConsumer<ProductChangeEvent>
    {
        private readonly IRepository<ProductItem> _productItemRepository;

        private readonly IRepository<StockItem> _stockItemRepository;

        private readonly ILogger<ProductItemChangeConsumer> _logger;

        public ProductItemChangeConsumer(IRepository<ProductItem> productItemRepository, IRepository<StockItem> stockItemRepository, ILogger<ProductItemChangeConsumer> llogger)
        {
            _productItemRepository = productItemRepository;
            _stockItemRepository = stockItemRepository;
            _logger = llogger;
        }

        public async Task Consume(ConsumeContext<ProductChangeEvent> context)
        {
            try
            {
                var message = context.Message;

                Expression<Func<ProductItem, bool>> filter = x => x.Id == message.Id;

                var ProductItem = new ProductItem
                {
                    Id = message.Id,
                    Name = message.Name,
                    Description = message.Description,
                };

                if (await _productItemRepository.ExistAsync(filter))
                {
                    await _productItemRepository.UpdateAsync(ProductItem.Id, ProductItem);

                    _logger.LogInformation($"[ASYNC-EVENT] - The product has been updated with id {message.Id}");
                }
                else
                {
                    var stockItem = new StockItem
                    {
                        ProductId = message.Id,
                        Stock = 0,
                        LastUpdate = DateTime.UtcNow
                    };

                    await _productItemRepository.CreateAsync(ProductItem);

                    _logger.LogInformation($"[ASYNC-EVENT] - The productItem has been created with id {message.Id}");

                    await _stockItemRepository.CreateAsync(stockItem);

                    _logger.LogInformation($"[ASYNC-EVENT] - The stockItem has been created with id {message.Id}");
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError($"[ASYNC-EVENT] - An unexpected error has occurred - {ex.Message}");
            }
        }        
    }
}
