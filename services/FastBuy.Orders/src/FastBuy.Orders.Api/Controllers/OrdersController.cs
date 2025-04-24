using FastBuy.Orders.Contracts.DTOs;
using FastBuy.Orders.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace FastBuy.Orders.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public IActionResult Create(OrderRequestDto orderRequestDto)
        {
            _orderService.CreateAsync(orderRequestDto);

            return Accepted();
        }
    }
}
