using FastBuy.Orders.Contracts.Events;
using FastBuy.Shared.Library.Repository.Abstractions;
using FastBuy.Stocks.Contracts.Events;
using FastBuy.Stocks.Entities;
using FastBuy.Stocks.Services.Exceptions;
using MassTransit;
using System.Linq.Expressions;

namespace FastBuy.Stocks.Services.Consumers;

public class DecreaseStockConsumer : IConsumer<StockDecreaseEvent>
{
    private readonly IRepository<StockItem> _stockRepository;

    public DecreaseStockConsumer(IRepository<StockItem> stockRepository)
    {
        _stockRepository = stockRepository;
    }

    public async Task Consume(ConsumeContext<StockDecreaseEvent> context)
    {
        try
        {
            var message = context.Message;

            foreach (var item in message.Items)
            {
                Expression<Func<StockItem, bool>> filter = x => x.ProductId == item.ProductId;

                var stockItem = await _stockRepository.GetAsync(filter)
                    ?? throw new NonExistentProductException(context.CorrelationId ?? Guid.Empty, $"The product with id {item.ProductId} does not exist");

                if (item.Quantity > stockItem.Stock)
                    throw new InsufficientStockException(context.CorrelationId ?? Guid.Empty, $"Insufficient stock for productId: {item.ProductId}");

                stockItem.Stock -= item.Quantity;

                await _stockRepository.UpdateAsync(stockItem.Id, stockItem);
            }

            var stockDecreasedEvent = new StockDecreasedEvent { CorrelationId = message.CorrelationId };

            await context.Publish(stockDecreasedEvent, ctx =>
            {
                ctx.CorrelationId = context.CorrelationId;
            });
        }
        catch (AsynchronousMessagingException ex)
        {
            var stockFailedDecrese = new StockFailedDecreaseEvent
            {
                CorrelationId = ex.CorrelationId,
                Reason = ex.Message
            };

            await context.Publish(stockFailedDecrese, ctx =>
            {
                ctx.CorrelationId = context.CorrelationId;
            });
        }
    }
}
