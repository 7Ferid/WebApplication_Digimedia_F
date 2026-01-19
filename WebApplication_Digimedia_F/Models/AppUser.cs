using Microsoft.AspNetCore.Identity;

namespace WebApplication_Digimedia_F.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
