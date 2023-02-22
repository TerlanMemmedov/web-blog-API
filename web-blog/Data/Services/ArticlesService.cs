using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using web_blog.Data.Enums;
using web_blog.Data.Models;
using web_blog.Data.ViewModels;

namespace web_blog.Data.Services
{
    public class ArticlesService
    {
        private readonly AppDbContext _context;
        //private readonly ArticlesSortingService _articlesSortingService;

        public ArticlesService(AppDbContext context)
        {
            _context = context;
        }

        //this is normal so we ordering by date added new ones top
        public async Task<List<ArticleWithAuthorComEmoVM>> GetAllArticles()
        {
            var articles = _context.Articles.Select(article1 => new ArticleWithAuthorComEmoVM
            {
                Id = article1.Id,
                Title = article1.Title,
                ArticleText = article1.ArticleText,
                Categories = article1.Categories.ToString(),
                NumberOfRead = article1.NumberOfRead,
                DateAdded = article1.DateAdded,
                //Author = article1.Author.NickName,
                User = article1.User.UserName,
                Comments = article1.Comments.Select(c => new CommentWArticleVM
                {
                    CommentText = c.CommentText,
                    DateAdded = c.DateAdded,
                    User = c.User.UserName
                }).ToList(),
                Emojis = article1.Emojis.Select(e => new EmojiWArticleVM
                {
                    EmojiTypes = e.EmojiTypes.ToString(),
                    User = e.User.UserName
                }).ToList()
            }).OrderByDescending(x => x.DateAdded).ToList();

            //Ordering
            //_articlesSortingService.SortByTimeAddedNewOnesOnTop(ref articles);

            //switch (sortBy)
            //{
            //    case "most-interacted":
            //        articles = articles.OrderByDescending(c => c.Comments.Count).ToList();
            //        break;
            //    case "most-viewed":
            //        articles = articles.OrderByDescending(v => v.NumberOfRead).ToList();
            //        break;
            //    default:
            //        break;
            //}

            return articles;
        }

        //Using for just Showing the article
        //Number of read melumat viewmodel cevrildikden sonra artirildigi ucun 1 say geriden gelir
        public async Task<ArticleWithAuthorComEmoVM> GetArticleByIdForShow(int id)
        {
            var article = _context.Articles.Where(n => n.Id == id).Select(article1 => new ArticleWithAuthorComEmoVM
            {
                Id = article1.Id,
                Title = article1.Title,
                ArticleText = article1.ArticleText,
                Categories = article1.Categories.ToString(),
                NumberOfRead = article1.NumberOfRead,
                DateAdded = article1.DateAdded,
                //Author = article1.Author.NickName,
                User = article1.User.UserName,
                Comments = article1.Comments.Select(c => new CommentWArticleVM
                {
                    CommentText = c.CommentText,
                    DateAdded = c.DateAdded,
                    User = c.User.UserName
                }).ToList(),
                Emojis = article1.Emojis.Select(e => new EmojiWArticleVM
                {
                    EmojiTypes = e.EmojiTypes.ToString(),
                    User = e.User.UserName
                }).ToList()
            }).FirstOrDefault();
            
            if (article != null)
            {
                await GetByIdForIncrement(id);
            }

            return article;
        }
        public async Task GetByIdForIncrement(int id)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(n => n.Id == id);

            article.NumberOfRead++;
            await _context.SaveChangesAsync();
        }

        //using for update and delete functions
        public async Task<Article> GetArtcileById(int id)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(n => n.Id == id);

            return article;
        }

        public async Task<Article> PostArticle(string userIDFOR, ArticleVM article)
        {
            //var author = await _authorsService.GetAuthorByUserIdAsync(userIDFOR);

            //if (author is null)
            //{
            //    return null;
            //}

            var newArticle = new Article()
            {
                Title = article.Title,
                ArticleText = article.ArticleText,
                Categories = article.Categories,
                DateAdded = DateTime.Now,
                //AuthorId = author.Id,
                UserId = userIDFOR
            };

            await _context.Articles.AddAsync(newArticle);
            await _context.SaveChangesAsync();

            return newArticle;
        }

        public async Task<Article> UpdateArticle(int articleId, string userid , ArticleVM article)
        {
            var _article = await _context.Articles.FirstOrDefaultAsync(n => n.Id == articleId);

            //var AuthorWhoWantToChangeArticle = await _authorsService.GetAuthorByUserIdAsync(userid);

            if (userid == _article.UserId)
            {
                if (_article != null)
                {
                    _article.Title = article.Title;
                    _article.ArticleText = article.ArticleText;
                    _article.Categories = article.Categories;

                    await _context.SaveChangesAsync();
                }
            }
            else
            {

            }

            return _article;
        }

        public async Task DeleteArticle(string userId, int articleId)
        {
            var article =  await _context.Articles.FirstOrDefaultAsync(n => n.Id == articleId);

            if (userId == article.UserId)  /// macrosa cevirmek lazimdi
            {
                if (article != null)
                {
                    _context.Articles.Remove(article);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteArticleByAdmin(int articleId)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(n => n.Id == articleId);

            if (article != null)
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }
        }
    }
}
