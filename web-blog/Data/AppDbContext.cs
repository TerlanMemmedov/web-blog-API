using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using web_blog.Data.Models;

namespace web_blog.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Emoji> Emojis { get; set; }
        public DbSet<ReportedArticle> ReportedArticles { get; set; }
        //public DbSet<Author> Authors { get; set; }

    }
}
