using System.ComponentModel.DataAnnotations;

namespace web_blog.Data.ViewModels
{
    public class CommentVM
    {
        [Required(ErrorMessage = "Comment text is required")]
        public string Text { get; set; }
    }
}
