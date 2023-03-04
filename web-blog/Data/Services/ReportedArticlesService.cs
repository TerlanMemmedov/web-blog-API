using Microsoft.EntityFrameworkCore;
using web_blog.Data.Enums;
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

        /// <summary>
        /// if there are error about articleId in the reportedArticles can be here
        /// and also look out for the model if want to go back change
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<int> GetArticleIdByReportId(int reportId)
        {
            var reportForArticleId = await _context.ReportedArticles.FirstOrDefaultAsync(a => a.Id == reportId);
            var articleId = -1;

            if (reportForArticleId != null && reportForArticleId.ArticleId != null)
            {
                articleId = (int)reportForArticleId.ArticleId;
            }

            return articleId;
        }

        //change status
        //bu zaman reportlar alinarken accepted ve declined olanlar ayri getirilsin
        public async Task ChangeStatus(int reportId, string statusMessage)
        {
            var reportForArticleId = await _context.ReportedArticles.FirstOrDefaultAsync(a => a.Id == reportId);
            int articleId = await GetArticleIdByReportId(reportId);

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
                            //for preventing auto deletion in the same time with article
                            report.ArticleId = null;

                            report.Status = Enums.RStatus.accepted;

                            report.IsDeleted = true;

                            await _context.SaveChangesAsync();

                            //burda her bir report sahibine notification gondermek qebulla bagli
                        }

                        //Email message to the user that their report accepted or Notifications model is coming

                        //bu problemin helli ucun meqalenin de report qebul edildikden 3 gun sonra silinmesi 
                        //lazimdir. Elave bir column elave edib orada deleted deyerini saxlayaraq
                        //, (silinecek vaxtida saxlamaq olar) 3 gun erzinde silmek
                    }
                    else if (statusMessage == "declined")
                    {
                        //foreach (var report in reportedOnes)
                        //{
                        //    report.Status = Enums.RStatus.declined;

                        //    await _context.SaveChangesAsync();
                        //}

                        //for preventing auto deletion in the same time with article
                        //reportForArticleId.ArticleId = null;
                        reportForArticleId.Status = Enums.RStatus.declined;

                        //delete this report within the 3 days 
                        reportForArticleId.IsDeleted = true;

                        await _context.SaveChangesAsync();

                        //Notification message to the user that their report declined 

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
                    ReportId = n.Id,
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
        //this is returning all the reported articles that without looking the status
        public async Task<List<ReportedArticlesWithUsersVM>> GetAllReportedArticles()
        {
            var reportedArticles = await _context.ReportedArticles.Select(n => new ReportedArticlesWithUsersVM
            {
                ReportId = n.Id,
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
        //this is returning all the reported articles that just checking status
        public async Task<List<ReportedArticlesWithUsersVM>> GetCheckingReportedArticles()
        {
            var reportedArticles = await _context.ReportedArticles.Where(k => k.Status == RStatus.checking).Select(n => new ReportedArticlesWithUsersVM
            {
                ReportId = n.Id,
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
                    ReportId = n.Id,
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


        //1 day deleting the reports that looked //
        public async Task DelTestTimer2()
        {
            var lookedReports = await _context.ReportedArticles.Where(n => n.IsDeleted == true).ToListAsync();

            if (lookedReports != null)
            {
                foreach (var lookedReport in lookedReports)
                {
                    _context.ReportedArticles.Remove(lookedReport);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
