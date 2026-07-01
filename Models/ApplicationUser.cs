using Microsoft.AspNetCore.Identity;

namespace mini_store.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = "";
    }
}