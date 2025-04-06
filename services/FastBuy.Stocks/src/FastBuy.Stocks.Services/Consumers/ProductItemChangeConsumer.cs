using FastBuy.Products.Contracts.Events;
using FastBuy.Shared.Library.Repository.Abstractions;
using FastBuy.Stocks.Entities;
using MassTransit;
using System.Linq.Expressions;

namespace FastBuy.Stocks.Services.Consumers
{
    public class ProductItemChangeConsumer : IConsumer<ProductChangeEvent>
    {
        private readonly IRepository<ProductItem> _repository;

        public ProductItemChangeConsumer(IRepository<ProductItem> repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<ProductChangeEvent> context)
        {
            var message = context.Message;

            Expression<Func<ProductItem, bool>> filter = x => x.Id == message.Id;

            var ProductItem = new ProductItem
            {
                Id = message.Id,
                Name = message.Name,
                Description = message.Description,
            };

            if (await _repository.ExistAsync(filter))
            {
                await _repository.UpdateAsync(ProductItem.Id, ProductItem);
            }
            else
            {
                await _repository.CreateAsync(ProductItem);
            }
        }        
    }
}
