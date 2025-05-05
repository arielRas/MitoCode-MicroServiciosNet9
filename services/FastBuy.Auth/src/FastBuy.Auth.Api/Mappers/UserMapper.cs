using FastBuy.Auth.Api.Contracts;
using FastBuy.Auth.Api.Entities;

namespace FastBuy.Auth.Api.Mappers
{
    internal static class UserMapper
    {
        public static UserResponseDto ToDto(this AppUser entity)
        {
            return new UserResponseDto
            {
                Id = Guid.Parse(entity.Id),
                Name = entity.Name,
                LastName = entity.LastName,
                Email = entity.Email!
            };
        }
    }
}
