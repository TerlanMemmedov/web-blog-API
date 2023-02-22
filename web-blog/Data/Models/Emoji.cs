using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using web_blog.Data.Enums;

namespace web_blog.Data.Models
{
    public class Emoji
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Emoji")]
        public EmojiTypes EmojiTypes { get; set; }

        //Relations

        //Will add userID from AppUser
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }

        //Will add ArticleId from Article
        public int ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public Article Article { get; set; }

    }
}
