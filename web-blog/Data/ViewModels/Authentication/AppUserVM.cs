using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace web_blog.Data.ViewModels.Authentication
{
    public class AppUserVM : IdentityUser
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        //[MinLength(8, ErrorMessage = "Password has to be minimum 8 character length")]
        public string Password { get; set; }

        [Required(ErrorMessage = "About yourself is required")]
        public string About { get; set; }

        public string? Role { get; set; }
    }
}
