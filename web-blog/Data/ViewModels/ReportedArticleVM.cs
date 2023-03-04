using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace web_blog.Data.ViewModels
{
    public class ReportedArticleVM
    {
        [Required(ErrorMessage = "Reporting message is required")]
        public string ReportText { get; set; }
    }

    public class ReportedArticlesWithUsersVM 
    {
        [Required]
        public int ReportId { get; set; }

        [Required]
        [DisplayName("Report Message")]
        public string ReportText { get; set; }

        [DisplayName("Reported By User")]
        public string User { get; set; }

        [DisplayName("Report Status")]
        public string Status { get; set; }

        [DisplayName("Reported Article")]
        public ArticleWithAuthorComEmoVM? Article { get; set; }
    }

}
