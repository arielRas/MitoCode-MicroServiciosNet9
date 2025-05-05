using FastBuy.Products.Contracts.DTOs;
using FastBuy.Products.Services.Abstractions;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FastBuy.Products.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]    
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService, IPublishEndpoint publishEndpoint)
        {
            _productService = productService;
        }


        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<ProductResponseDto>>GetById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("The id field cannot be empty");

                var product = await _productService.GetByIdAsync(id);

                return Ok(product);
            }
            catch(Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAll()
        {
            try
            {                
                var products = await _productService.GetAllAsync();

                return Ok(products);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> Create([FromBody] ProductRequestDto product)
        {
            try
            {
                if (product is null)
                    return BadRequest("The resource to be inserted cannot be null");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var newProduct = await _productService.CreateAsync(product);

                return CreatedAtAction(nameof(GetById), new { Id = newProduct.Id }, newProduct);
            }
            catch(Exception)
            {
                throw;
            }           
        }


        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ProductRequestDto product)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("The id field cannot be empty");

                if (product is null)
                    return BadRequest("The resource to be inserted cannot be null");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _productService.UpdateAsync(id, product);

                return NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("The id field cannot be empty");

                await _productService.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
