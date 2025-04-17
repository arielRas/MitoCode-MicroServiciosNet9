using Duende.IdentityServer;
using FastBuy.Auth.Api.Configurations;
using FastBuy.Auth.Api.Contracts;
using FastBuy.Auth.Api.Entities;
using FastBuy.Auth.Api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FastBuy.Auth.Api.Controllers
{    
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName, Roles = Role.Admin)]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UsersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }


        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<UserResponseDto>> GetById([FromRoute] Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
                return NotFound($"User with id {id} does not exist");

            return Ok(user.ToDto());
        }


        [HttpGet("{email}")]
        public async Task<ActionResult<UserResponseDto>> GetByEmail([FromRoute] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return NotFound($"User with email {email} does not exist");

            return Ok(user.ToDto());
        }


        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody]UserResponseDto userDto)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
                return NotFound($"User with id {id} does not exist");

            user.Name = userDto.Name;
            user.LastName = userDto.LastName;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError,(result.Errors));

            return NoContent();
        }


        [HttpPatch("{id:Guid}")]
        public async Task<IActionResult> UpdateEmail([FromRoute] Guid id, [FromBody] string email)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
                return NotFound($"User with id {id} does not exist");

            var result = await _userManager.SetEmailAsync(user, email);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, (result.Errors));

            return NoContent();
        }


        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
                return NotFound($"User with id {id} does not exist");

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, (result.Errors));

            return NoContent();
        }
    }
}
