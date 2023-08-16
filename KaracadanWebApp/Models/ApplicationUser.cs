
using Microsoft.AspNetCore.Identity;

namespace KaracadanWebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public long PhoneNumber { get; set; }
        public string? Address { get; set; }
        public ICollection<Order>? Orders { get; set; }
       
        
    }
}
