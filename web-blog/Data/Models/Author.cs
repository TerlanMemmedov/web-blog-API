//using Microsoft.AspNetCore.Identity;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace web_blog.Data.Models
//{
//    public class Author
//    {
//        [Key]
//        public int Id { get; set; }

//        [Required]
//        public string NickName { get; set; }

//        //Relations

//        //Will add UserId from AppUser
//        public string? UserId { get; set; }
//        [ForeignKey("UserId")]
//        public virtual ApplicationUser? User { get; set; }

//        //List Article nullable
//        public List<Article>? Articles { get; set; }

//    }
//}
