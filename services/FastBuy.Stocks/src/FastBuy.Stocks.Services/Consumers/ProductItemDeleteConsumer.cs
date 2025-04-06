using FastBuy.Products.Contracts.Events;
using FastBuy.Shared.Library.Repository.Abstractions;
using FastBuy.Stocks.Entities;
using MassTransit;
using System.Linq.Expressions;

namespace FastBuy.Stocks.Services.Consumers
{
    public class ProductItemDeleteConsumer : IConsumer<ProductDeletedEvent>
    {
        private readonly IRepository<ProductItem> _repository;

        public ProductItemDeleteConsumer(IRepository<ProductItem> repository)
        {
            _repository = repository;
        }
        public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
        {
            var message = context.Message;

            Expression<Func<ProductItem, bool>> filter = x => x.Id == message.Id;

            if (!await _repository.ExistAsync(filter)) return;

            await _repository.DeleteAsync(message.Id);
        }
    }
}
