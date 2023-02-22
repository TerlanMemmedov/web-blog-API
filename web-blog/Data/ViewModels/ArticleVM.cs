using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using web_blog.Data.Enums;
using web_blog.Data.Models;

namespace web_blog.Data.ViewModels
{
    public class ArticleVM
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Text of the article is required")]
        public string ArticleText { get; set; }

        [Required(ErrorMessage = "Article category is required")]
        public Categories Categories { get; set; } //have to be changed to List<C> C, works with params

    }
    public class ArticleWithAuthorComEmoVM
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string ArticleText { get; set; }

        [Required]
        public string Categories { get; set; } //can be changed to List<C> C

        public int NumberOfRead { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;

        //Relations

        //Will add AuthorID such as Author class and takes userId from AppUser
        public virtual string User { get; set; }


        //Will add List comment Nullable
        public List<CommentWArticleVM>? Comments { get; set; }


        //Will add List emojis Nullable
        public List<EmojiWArticleVM>? Emojis { get; set; }
    }

    public class CommentWArticleVM
    {
        public string CommentText { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;

        public virtual string User { get; set; }
    }

    public class EmojiWArticleVM
    {
        public string EmojiTypes { get; set; }

        public virtual string? User { get; set; }
    }
}
