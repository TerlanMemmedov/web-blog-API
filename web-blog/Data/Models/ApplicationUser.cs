using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace web_blog.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? About { get; set; }

        //public virtual List<Author>? Authors { get; set; }
        public virtual List<Comment>? Comments { get; set; }
        public virtual List<Emoji>? Emojis { get; set; }

        [DefaultValue(0)]
        public virtual int NumberOfAllReportedThings { get; set; }

        [DefaultValue(0)]
        public virtual int NumberOfLastReportedThings { get; set; } //after block return to 0 again
    }
}
