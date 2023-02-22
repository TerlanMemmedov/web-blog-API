using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_blog.Data.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Comment Text")]
        public string CommentText { get; set; }
        
        public DateTime DateAdded { get; set; } = DateTime.Now;

        //Relations 


        //Will add Article id as foreign key
        public int ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public Article Article { get; set; }


        //Will add userId which wrote the comment id as foreign key
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
        

    }
}
