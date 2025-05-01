using FastBuy.Shared.Events.Exceptions;
using FastBuy.Shared.Events.Saga.Orders;
using FastBuy.Shared.Events.Saga.Stocks;
using FastBuy.Shared.Library.Repository.Abstractions;
using FastBuy.Stocks.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace FastBuy.Stocks.Services.Consumers;

public class StockDecreaseConsumer : IConsumer<StockDecreaseRequestedEvent>
{
    private readonly IRepository<StockItem> _stockRepository;
    private readonly ILogger<StockDecreaseConsumer> _logger;

    public StockDecreaseConsumer(IRepository<StockItem> stockRepository, ILogger<StockDecreaseConsumer> logger)
    {
        _stockRepository = stockRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<StockDecreaseRequestedEvent> context)
    {
        var message = context.Message;

        var stockItemsDiscounted = new List<OrderItem>();

        try
        {
            foreach (var item in message.Items)
            {
                Expression<Func<StockItem, bool>> filter = x => x.ProductId == item.ProductId;

                var stockItem = await _stockRepository.GetAsync(filter)
                    ?? throw new NonExistentProductException(message.CorrelationId, $"The product with id {item.ProductId} does not exist");

                if (item.Quantity > stockItem.Stock)
                    throw new InsufficientStockException(message.CorrelationId, $"Insufficient stock for productId: {item.ProductId}");

                stockItem.Stock -= item.Quantity;

                await _stockRepository.UpdateAsync(stockItem.Id, stockItem);

                stockItemsDiscounted.Add(item);
            }

            var stockDecreasedEvent = new StockDecreasedEvent 
            {
                CorrelationId = message.CorrelationId 
            };

            await context.Publish(stockDecreasedEvent, ctx =>
            {
                ctx.CorrelationId = context.CorrelationId;
            });

            _logger.LogInformation($"[SAGA] - Generate {nameof(StockDecreasedEvent)}");
        }
        catch (AsynchronousMessagingException ex)
        {
            _logger.LogError($"[SAGA] - Error: {ex.Message}");

            var stockFailedDecrese = new StockDecreaseFailedEvent
            {
                CorrelationId = ex.CorrelationId,
                DiscountedItems = stockItemsDiscounted,
                NonDiscountedItems = message.Items.Except(stockItemsDiscounted),
                Reason = ex.Message
            };

            await context.Publish(stockFailedDecrese, ctx =>
            {
                ctx.CorrelationId = context.CorrelationId;
            });

            _logger.LogInformation($"[SAGA] - Generate {nameof(StockDecreaseFailedEvent)} - Reason: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"[SAGA] - {DateTime.UtcNow} - Error when trying to decrease stock, correlationID: {message.CorrelationId} - {ex.Message}", ex);
        }
    }    
}