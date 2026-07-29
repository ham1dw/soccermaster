using Microsoft.AspNetCore.Identity;

namespace soccer.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}