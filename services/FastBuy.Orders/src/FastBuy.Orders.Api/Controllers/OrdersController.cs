using FastBuy.Orders.Contracts.DTOs;
using FastBuy.Orders.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastBuy.Orders.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderRequestDto orderRequestDto)
        {
            await _orderService.CreateAsync(orderRequestDto);

            return Accepted();
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<OrderResponseDto>> GetById(Guid id)
        {
            var order = await _orderService.GetByIdAsync(id);

            return Ok(order);
        }
    }
}
