using System.ComponentModel.DataAnnotations;

namespace web_blog.Data.ViewModels.Authentication
{
    public class LoginVM
    {
        [Required(ErrorMessage = "This section is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This section is required")]
        public string PassWord { get; set; }  
    }
}
