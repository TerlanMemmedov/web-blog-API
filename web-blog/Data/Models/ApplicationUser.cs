using Microsoft.AspNetCore.Identity;

namespace web_blog.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? About { get; set; }

        //public virtual List<Author>? Authors { get; set; }
        public virtual List<Comment>? Comments { get; set; }
        public virtual List<Emoji>?  Emojis { get; set; }
    }
}
