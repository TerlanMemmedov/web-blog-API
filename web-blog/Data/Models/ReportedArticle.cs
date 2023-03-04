using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using web_blog.Data.Enums;

namespace web_blog.Data.Models
{
    public class ReportedArticle
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ArticleId { get; set; }
        [ForeignKey(nameof(ArticleId))]
        public Article Article { get; set; }

        [Required]
        public string ReportText { get; set; }
        [Required]
        public DateTime DateReported { get; set; }

        [Required]
        public virtual string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }

        //will add 
        //report durumu (enum) qebul edildi, edilmedi, baxilir
        public RStatus Status { get; set; }


        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}
