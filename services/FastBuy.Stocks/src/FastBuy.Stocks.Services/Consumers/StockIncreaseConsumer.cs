using FastBuy.Shared.Events.Exceptions;
using FastBuy.Shared.Events.Saga.Orders;
using FastBuy.Shared.Events.Saga.Stocks;
using FastBuy.Shared.Library.Repository.Abstractions;
using FastBuy.Stocks.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace FastBuy.Stocks.Services.Consumers
{
    public class StockIncreaseConsumer : IConsumer<StockIncreaseRequestedEvent>
    {
        private readonly IRepository<StockItem> _stockRepository;
        private readonly ILogger<StockIncreaseConsumer> _logger;

        public StockIncreaseConsumer(IRepository<StockItem> stockRepository, ILogger<StockIncreaseConsumer> logger)
        {
            _stockRepository = stockRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<StockIncreaseRequestedEvent> context)
        {
            var message = context.Message;

            var stockItemsIncreased = new List<OrderItem>();

            try 
            {
                foreach (var item in message.Items)
                {
                    Expression<Func<StockItem, bool>> filter = x => x.ProductId == item.ProductId;

                    var stockItem = await _stockRepository.GetAsync(filter)
                        ?? throw new NonExistentProductException(message.CorrelationId, $"The product with id {item.ProductId} does not exist");

                    stockItem.Stock += item.Quantity;

                    await _stockRepository.UpdateAsync(stockItem.Id, stockItem);

                    stockItemsIncreased.Add(item);
                }

                var stockDecreasedEvent = new StockIncreasedEvent
                {
                    CorrelationId = message.CorrelationId,
                };

                await context.Publish(stockDecreasedEvent, ctx =>
                {
                    ctx.CorrelationId = context.CorrelationId;
                });

                _logger.LogInformation($"[SAGA] - Generate {nameof(StockIncreasedEvent)} - CorrelationId; {message.CorrelationId}");
            }
            catch (AsynchronousMessagingException ex)
            {  
                _logger.LogError($"[SAGA] - Error in asynchronous communication - {ex.Message}");

                var stockIncreasedFailedEvent = new StockIncreaseFailedEvent
                {
                    CorrelationId = ex.CorrelationId,
                    IncreadedItems = stockItemsIncreased,
                    NonIncreadedItems = message.Items.Except(stockItemsIncreased),
                    Reason = ex.Message
                };

                await context.Publish(stockIncreasedFailedEvent, ctx =>
                {
                    ctx.CorrelationId = context.CorrelationId;
                });               

                _logger.LogInformation($"[SAGA] - Send {nameof(StockIncreaseFailedEvent)}");

            }
            catch (Exception ex)
            {
                _logger.LogError($"[SAGA] - {DateTime.UtcNow} - Error when trying to decrease stock, correlationID: {message.CorrelationId} - {ex.Message}", ex);
            }
        }
    }
}
