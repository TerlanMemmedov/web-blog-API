using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using web_blog.Data.Enums;

namespace web_blog.Data.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Length of te Title have to be minimum 3 characters long")]
        public string Title { get; set; }

        [Required]
        [Column(TypeName = "varchar(MAX)")]
        [DisplayName("Article Text")]
        public string ArticleText { get; set; }

        [Required]
        public Categories Categories { get; set; } //can be changed to List<C> C
        
        [DefaultValue(0)] 
        public int NumberOfRead { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;

        //Relations

        //Will add AuthorID such as Author class and takes userId from AppUser

        //public int AuthorId { get; set; }
        //[ForeignKey("AuthorId")]
        //public Author Author { get; set; }

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }



        //Will add List comment Nullable
        public List<Comment>? Comments { get; set; }

        
        //Will add List emojis Nullable
        public List<Emoji>? Emojis { get; set; }
    }
}
