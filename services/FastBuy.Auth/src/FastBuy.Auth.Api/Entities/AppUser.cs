using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FastBuy.Auth.Api.Entities
{
    public class AppUser : IdentityUser 
    {
        [Required]
        [MaxLength(70)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public required string LastName { get; set; }
    }
}
