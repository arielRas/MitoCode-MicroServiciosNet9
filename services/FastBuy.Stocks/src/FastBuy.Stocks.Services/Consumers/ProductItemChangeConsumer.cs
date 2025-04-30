using FastBuy.Shared.Events.Events.Products;
using FastBuy.Shared.Library.Repository.Abstractions;
using FastBuy.Stocks.Entities;
using MassTransit;
using System.Linq.Expressions;

namespace FastBuy.Stocks.Services.Consumers
{
    public class ProductItemChangeConsumer : IConsumer<ProductChangeEvent>
    {
        private readonly IRepository<ProductItem> _productItemRepository;

        private readonly IRepository<StockItem> _stockItemRepository;

        public ProductItemChangeConsumer(IRepository<ProductItem> productItemRepository, IRepository<StockItem> stockItemRepository)
        {
            _productItemRepository = productItemRepository;
            _stockItemRepository = stockItemRepository;
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

            if (await _productItemRepository.ExistAsync(filter))
            {
                await _productItemRepository.UpdateAsync(ProductItem.Id, ProductItem);
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

                await _stockItemRepository.CreateAsync(stockItem);
            }
        }        
    }
}
