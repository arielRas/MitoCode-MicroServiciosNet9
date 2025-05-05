using FastBuy.Auth.Api.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FastBuy.Auth.Api.DataAccess
{
    public class AuthDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
            
        }
    }
}
