using FastBuy.Orders.Contracts.DTOs;
using FastBuy.Orders.Repository.Repositories.Abstractions;
using FastBuy.Orders.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBuy.Orders.Services.Implementations
{
    class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Task CreateAsync(OrderRequestDto orderDto)
        {
            throw new NotImplementedException();
        }
    }
}
