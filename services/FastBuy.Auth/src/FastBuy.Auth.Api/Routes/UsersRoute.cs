using FastBuy.Auth.Api.Contracts;
using FastBuy.Auth.Api.Entities;
using FastBuy.Auth.Api.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FastBuy.Auth.Api.Routes
{
    public static class UsersRoute
    {
        public static IApplicationBuilder MapUsersRoutes(this WebApplication app)
        {
            var group = app.MapGroup("/api/users").WithTags("Users");

            group.MapGet("/{id:Guid}", GetById);

            group.MapGet("/{email}", GetByEmail);

            group.MapPut("/{id:Guid}", Update);

            group.MapPatch("/{id:Guid}", UpdateEmail);

            group.MapDelete("/{id:Guid}", Delete);

            return app;
        }


        private async static Task<IResult> GetById([FromRoute]Guid id, UserManager<AppUser> userManager)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user is null)
                return Results.NotFound($"User with id {id} does not exist");

            return Results.Ok(user.ToDto());
        }

        private async static Task<IResult> GetByEmail([FromRoute] string email, UserManager<AppUser> userManager)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
                return Results.NotFound($"User with email {email} does not exist");

            return Results.Ok(user.ToDto());
        }

        private async static Task<IResult> Update([FromRoute] Guid id, [FromBody] UserUpdateRequestDto userDto, UserManager<AppUser> userManager)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user is null)
                return Results.NotFound($"User with id {id} does not exist");

            user.Name = userDto.Name;
            user.LastName = userDto.Lastname;

            var result = await userManager.UpdateAsync(user);

            if(!result.Succeeded)
                return Results.InternalServerError(result.Errors);

            return Results.NoContent();
        }

        private async static Task<IResult> UpdateEmail([FromRoute] Guid id, [FromBody] string email, UserManager<AppUser> userManager)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user is null)
                return Results.NotFound($"User with id {id} does not exist");

            var result = await userManager.SetEmailAsync(user, email);

            if (!result.Succeeded)
                return Results.InternalServerError(result.Errors);

            return Results.NoContent();
        }

        private async static Task<IResult> Delete([FromRoute] Guid id, UserManager<AppUser> userManager)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user is null)
                return Results.NotFound($"User with id {id} does not exist");

            var result = await userManager.DeleteAsync(user);

            if (!result.Succeeded)
                return Results.InternalServerError(result.Errors);

            return Results.NoContent();
        }
    }
}
