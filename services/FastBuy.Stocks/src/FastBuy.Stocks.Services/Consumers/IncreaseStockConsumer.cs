using FastBuy.Shared.Library.Repository.Abstractions;
using FastBuy.Stocks.Contracts.Events;
using FastBuy.Stocks.Entities;
using FastBuy.Stocks.Services.Exceptions;
using MassTransit;
using System.Linq.Expressions;

namespace FastBuy.Stocks.Services.Consumers
{
    public class IncreaseStockConsumer : IConsumer<IncreaseStockEvent>
    {
        private readonly IRepository<StockItem> _stockRepository;
        private readonly IRepository<ProductItem> _productRepository;

        public IncreaseStockConsumer(IRepository<StockItem> stockRepository, IRepository<ProductItem> productRepository)
        {
            _stockRepository = stockRepository;
            _productRepository = productRepository;
        }

        public async Task Consume(ConsumeContext<IncreaseStockEvent> context)
        {
            var message = context.Message;

            Expression<Func<StockItem, bool>> filter = x => x.ProductId == message.ProductId;

            var stockItem = await _stockRepository.GetAsync(filter)
                ?? throw new NonExistentProductException(message.ProductId);

            stockItem.Stock += message.Quantity;

            await _stockRepository.UpdateAsync(stockItem.Id, stockItem);

            await context.Publish (new IncreasedStockEvent { CorrelationId = message.CorrelationId });
        }
    }
}
