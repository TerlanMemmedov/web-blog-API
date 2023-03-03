using Microsoft.EntityFrameworkCore;
using web_blog.Data.Models;
using web_blog.Data.ViewModels;
using web_blog.Data.ViewModels.Authentication;

namespace web_blog.Data.Services
{
    public class ReportedArticlesService
    {
        private readonly AppDbContext _context;

        public ReportedArticlesService(AppDbContext context)
        {
            _context = context;
        }

        public async Task ReportArticle(int id, string userId, ReportedArticleVM reportedArticleVM)
        {
            var report = new ReportedArticle()
            {
                ArticleId = id,
                UserId = userId,
                DateReported = DateTime.Now,
                ReportText = reportedArticleVM.ReportText,
                Status = Enums.RStatus.checking
            };

            await _context.ReportedArticles.AddAsync(report);
            await _context.SaveChangesAsync();
        }

        //No type conversion. Just for find with given article id
        public async Task<List<ReportedArticle>> FindReportesWithGivenArticleID(int articleId)
        {
            var reports = await _context.ReportedArticles.Where(n => n.ArticleId == articleId).ToListAsync();

            return reports;
        }

        public async Task<int> GetArticleIdByReportId(int reportId)
        {
            var reportForArticleId = await _context.ReportedArticles.FirstOrDefaultAsync(a => a.Id == reportId);
            var articleId = -1;

            if (reportForArticleId != null)
            {
                articleId = reportForArticleId.ArticleId;
            }
            

            return articleId;
        }

        //change status
        //bu zaman reportlar alinarken accepted ve declined olanlar ayri getirilsin
        public async Task ChangeStatus(int reportId, string statusMessage)
        {
            var reportForArticleId = await _context.ReportedArticles.FirstOrDefaultAsync(a => a.Id == reportId);
            int articleId =await GetArticleIdByReportId(reportId);

            if (articleId != null)
            {
                var reportedOnes = await FindReportesWithGivenArticleID(articleId);

                if (reportedOnes is null)
                {

                }
                else
                {
                    if (statusMessage == "accepted")
                    {
                        foreach (var report in reportedOnes)
                        {
                            report.Status = Enums.RStatus.accepted;

                            report.IsDeleted = true;

                            await _context.SaveChangesAsync();

                            //burda her bir report sahibine email gondermek qebulla bagli
                        }

                        //Email message to the user that their report accepted

                        //Sorry but can't wait for 3 days. Because it is all connected. So when article is 
                        //deleted report will also deleted (all reports about this article)

                        //bu problemin helli ucun meqalenin de report qebul edildikden 3 gun sonra silinmesi 
                        //lazimdir. Elave bir column elave edib orada deleted deyerini saxlayaraq, (silinecek vaxti
                        //da saxlamaq olar) 3 gun erzinde silmek
                    }
                    else if (statusMessage == "declined")
                    {
                        //foreach (var report in reportedOnes)
                        //{
                        //    report.Status = Enums.RStatus.declined;

                        //    await _context.SaveChangesAsync();
                        //}

                        reportForArticleId.Status = Enums.RStatus.declined;

                        //delete this report within the 3 days 
                        reportForArticleId.IsDeleted = true;

                        await _context.SaveChangesAsync();

                        //Email message to the user that their report declined ???

                    }
                }
            }
        }

        //For User
        public async Task<List<ReportedArticlesWithUsersVM>> GetAllReportedArticlesOfUser(string userId)
        {
            var reportedArticles = await _context.ReportedArticles.Where(z => z.UserId == userId)
                .Select(n => new ReportedArticlesWithUsersVM
            {
                User = n.User.UserName,
                ReportText = n.ReportText,
                Status = n.Status.ToString(),
                Article = new ArticleWithAuthorComEmoVM()
                {
                    Id = n.Article.Id,
                    Title = n.Article.Title,
                    ArticleText = n.Article.ArticleText,
                    Categories = n.Article.Categories.ToString(),
                    NumberOfRead = n.Article.NumberOfRead,
                    DateAdded = n.Article.DateAdded,
                    //Author = article1.Author.NickName,
                    User = n.Article.User.UserName,
                    Comments = n.Article.Comments.Select(c => new CommentWArticleVM
                    {
                        CommentText = c.CommentText,
                        DateAdded = c.DateAdded,
                        User = c.User.UserName
                    }).ToList(),
                    Emojis = n.Article.Emojis.Select(e => new EmojiWArticleVM
                    {
                        EmojiTypes = e.EmojiTypes.ToString(),
                        User = e.User.UserName
                    }).ToList()
                }
            }).ToListAsync();

            return reportedArticles;
        }


        //For admin
        public async Task<List<ReportedArticlesWithUsersVM>> GetAllReportedArticles()
        {
            var reportedArticles = await _context.ReportedArticles.Select(n => new ReportedArticlesWithUsersVM
            {
                User = n.User.UserName,
                ReportText = n.ReportText,
                Status = n.Status.ToString(),
                Article = new ArticleWithAuthorComEmoVM()
                {
                    Id = n.Article.Id,
                    Title = n.Article.Title,
                    ArticleText = n.Article.ArticleText,
                    Categories = n.Article.Categories.ToString(),
                    NumberOfRead = n.Article.NumberOfRead,
                    DateAdded = n.Article.DateAdded,
                    //Author = article1.Author.NickName,
                    User = n.Article.User.UserName,
                    Comments = n.Article.Comments.Select(c => new CommentWArticleVM
                    {
                        CommentText = c.CommentText,
                        DateAdded = c.DateAdded,
                        User = c.User.UserName
                    }).ToList(),
                    Emojis = n.Article.Emojis.Select(e => new EmojiWArticleVM
                    {
                        EmojiTypes = e.EmojiTypes.ToString(),
                        User = e.User.UserName
                    }).ToList()
                }
            }).ToListAsync();

            return reportedArticles;
        }


        //For admin
        public async Task<ReportedArticlesWithUsersVM> GetReportedArticleById(int id)
        {
            var reportedArticle = await _context.ReportedArticles.Where(a => a.Id == id)
                .Select(n => new ReportedArticlesWithUsersVM
                {
                    User = n.User.UserName,
                    ReportText = n.ReportText,
                    Status = n.Status.ToString(),
                    Article = new ArticleWithAuthorComEmoVM()
                    {
                        Id = n.Article.Id,
                        Title = n.Article.Title,
                        ArticleText = n.Article.ArticleText,
                        Categories = n.Article.Categories.ToString(),
                        NumberOfRead = n.Article.NumberOfRead,
                        DateAdded = n.Article.DateAdded,
                        //Author = article1.Author.NickName,
                        User = n.Article.User.UserName,
                        Comments = n.Article.Comments.Select(c => new CommentWArticleVM
                        {
                            CommentText = c.CommentText,
                            DateAdded = c.DateAdded,
                            User = c.User.UserName
                        }).ToList(),
                        Emojis = n.Article.Emojis.Select(e => new EmojiWArticleVM
                        {
                            EmojiTypes = e.EmojiTypes.ToString(),
                            User = e.User.UserName
                        }).ToList()
                    }
                }).FirstOrDefaultAsync();

            return reportedArticle;
        }
    }
}
