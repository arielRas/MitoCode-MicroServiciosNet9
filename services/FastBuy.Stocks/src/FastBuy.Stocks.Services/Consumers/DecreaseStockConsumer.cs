using FastBuy.Shared.Library.Repository.Abstractions;
using FastBuy.Stocks.Contracts.Events;
using FastBuy.Stocks.Entities;
using FastBuy.Stocks.Services.Exceptions;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FastBuy.Stocks.Services.Consumers
{
    public class DecreaseStockConsumer : IConsumer<DecreaseStockEvent>
    {
        private readonly IRepository<StockItem> _stockRepository;
        private readonly IRepository<ProductItem> _productRepository;

        public DecreaseStockConsumer(IRepository<StockItem> stockRepository, IRepository<ProductItem> productRepository)
        {
            _stockRepository = stockRepository;
            _productRepository = productRepository;
        }

        public async Task Consume(ConsumeContext<DecreaseStockEvent> context)
        {
            var message = context.Message;

            Expression<Func<StockItem, bool>> filter = x => x.ProductId == message.ProductId;

            var stockItem = await _stockRepository.GetAsync(filter) 
                ?? throw new NonExistentProductException(message.ProductId);

            stockItem.Stock -= message.Quantity;

            await _stockRepository.UpdateAsync(stockItem.Id, stockItem);

            await context.Publish( new DecreasedStockEvent { CorrelationId = message.CorrelationId });
        }
    }
}
