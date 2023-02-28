using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using System.Net;
using web_blog.Data.Enums;
using web_blog.Data.Errors;
using web_blog.Data.Services;
using web_blog.Data.ViewModels;
using web_blog.Data.ViewModels.Authentication;

namespace web_blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ArticlesService _service;
        private readonly CommentsService _commentsService;
        private readonly EmojisService _emojisService;
        private readonly ReportedArticlesService _reportedArticlesService;

        public ArticlesController(ArticlesService service, 
            CommentsService commentsService, EmojisService emojisService,
            ReportedArticlesService reportedArticlesService)
        {
            _service = service;
            _commentsService = commentsService;
            _emojisService = emojisService;
            _reportedArticlesService = reportedArticlesService;
        }


        //report article function and model will be added



        //Will add filtering and change the actions like if case in the server for chosen one
        //all can
        //Normal get articles works ordering by the new ones top
        [HttpGet("")]
        public async Task<IActionResult> GetAllArticles() 
        {
            var articles = await _service.GetAllArticles();

            return Ok(articles);
        }

        //?
        //[HttpGet("sort-type={sortBy}")]
        //public async Task<IActionResult> GetAllArticlesSorted(string? sortBy)
        //{
        //    var articles = await _service.GetAllArticles(sortBy);

        //    return Ok(articles);
        //}


        //Testing Redis
        [HttpGet("thisIs/Redis")]
        public async Task<IActionResult> GetTaskRedis()
        {
            var articles = await _service.TestRedisAllInfoArticles();

            return Ok(articles);
        }



        //all can
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticle(int id)
        {
            var article = await _service.GetArticleByIdForShow(id);

            if (article is null)
            {
                return NotFound(ErrorFormat.GetErrorJsonType(NotFound().StatusCode , ErrorNamesForMessages.ArticleNotFound));

                //return NotFound("Article couldn't found");
            }

            return Ok(article);
        }

        //Need user role 
        //Also admin
        [Authorize(Roles = UserRoles.User + "," + UserRoles.Admin)]
        [HttpPost("post-article")]
        public async Task<IActionResult> PostArticle([FromBody] ArticleVM article)
        {
            var userId = Request.Cookies["userId"];

            if (userId is null)
            {
                //return BadRequest("Problem with the user");
                return BadRequest(ErrorFormat.GetErrorJsonType(BadRequest().StatusCode, ErrorNamesForMessages.ProblemWithUser));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ValidationState);
            }

            var postedArticle = await _service.PostArticle(userId, article);

            if (postedArticle is null)
            {
                return Conflict(ErrorFormat.GetErrorJsonType(Conflict().StatusCode, ErrorNamesForMessages.ArticleCantPosted));
            }

            return Ok(postedArticle);
        }


        //need user role 
        //just exatly the author of the chosen article
        [Authorize(Roles = UserRoles.User)]
        [HttpPut("edit-article/{articleId}")]
        public async Task<IActionResult> UpdateArticle(int articleId, [FromBody] ArticleVM article)
        {
            //add user id for checking the if its author updates it
            var userid = Request.Cookies["userId"];

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ValidationState);
            }

            var _article = await _service.GetArtcileById(articleId);
            if (_article is null)
            {
                return NotFound(ErrorFormat.GetErrorJsonType(NotFound().StatusCode , ErrorNamesForMessages.ArticleNotFound));
            }

            var updatedarticle = await _service.UpdateArticle(articleId, userid , article);
            return Ok(updatedarticle);
        }

        //need user
        //just himself
        [Authorize(Roles = UserRoles.User)]
        [HttpDelete("delete-article/{articleId}")]
        public async Task<IActionResult> DeleteArticle(int articleId)
        {
            var userId = Request.Cookies["userId"];

            if (userId is null)
            {
                return BadRequest(ErrorFormat.GetErrorJsonType(BadRequest().StatusCode, ErrorNamesForMessages.ProblemWithUser));
            }

            var article = await _service.GetArtcileById(articleId);
            if (article is null)
            {
                return NotFound(ErrorFormat.GetErrorJsonType(NotFound().StatusCode, ErrorNamesForMessages.ArticleNotFound));
            }

            await _service.DeleteArticle(userId, articleId);
            return Ok("Article deleted");
        }


        //forcing for deletion by admin.
        //Service-de mesale sahibine message, or email gonderilmelidir
        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("delete-article-by-admin/{articleId}")]
        public async Task<IActionResult> ForceDeleteArticle(int articleId)
        {
            var article = await _service.GetArtcileById(articleId);

            if (article is null)
            {
                return NotFound(ErrorFormat.GetErrorJsonType(NotFound().StatusCode, ErrorNamesForMessages.ArticleNotFound));
            }

            await _service.DeleteArticleByAdmin(articleId);
            return Ok("Article deleted");
        }

        //only users can
        [HttpPost("{id}/post-comment")]
        [Authorize(Roles = UserRoles.User + "," + UserRoles.Admin)]
        public async Task<IActionResult> PostComment(int id, [FromBody] CommentVM comment)
        {
            var userId = Request.Cookies["userId"];

            if (userId is null)
                return BadRequest(ErrorFormat.GetErrorJsonType(BadRequest().StatusCode, ErrorNamesForMessages.ProblemWithUser));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ValidationState);
            }

            var article = await _service.GetArtcileById(id);

            if (article is null)
                return NotFound(ErrorFormat.GetErrorJsonType(NotFound().StatusCode, ErrorNamesForMessages.ArticleNotFound));

            await _commentsService.PostComment(id, comment, userId);

            return Ok("Comment posted to the article");
        }

        //only users can
        [HttpPost("{id}/post-emoji")]
        [Authorize(Roles = UserRoles.User + "," + UserRoles.Admin)]
        public async Task<IActionResult> PostEmoji(int id, [FromBody] EmojiVM emoji)
        {
            var userId = Request.Cookies["userId"];

            if (userId is null)
                return BadRequest(ErrorFormat.GetErrorJsonType(BadRequest().StatusCode, ErrorNamesForMessages.ProblemWithUser));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ValidationState);
            }

            var article = await _service.GetArtcileById(id);

            if (article is null)
                return NotFound(ErrorFormat.GetErrorJsonType(NotFound().StatusCode, ErrorNamesForMessages.ArticleNotFound));

            await _emojisService.PostEmoji(id, emoji, userId);

            return NoContent(); //"Emoji posted, updated or deleted for the article"
        }

        //the author of comment can edit his comment
        [HttpPut("{id}/edit-comment/{commentId}")]
        [Authorize(Roles = UserRoles.User + "," + UserRoles.Admin)]
        public async Task<IActionResult> UpdateComment(int id, int commentId, [FromBody] CommentVM commentVM)
        {
            var userId = Request.Cookies["userId"];

            if (userId is null)
                return BadRequest(ErrorFormat.GetErrorJsonType(BadRequest().StatusCode, ErrorNamesForMessages.ProblemWithUser));

            if (!ModelState.IsValid)
                return BadRequest(ModelState.ValidationState);

            var article = await _service.GetArtcileById(id);

            if (article is null)
                return NotFound(ErrorFormat.GetErrorJsonType(NotFound().StatusCode, ErrorNamesForMessages.ArticleNotFound));

            var updatedComment = await _commentsService.UpdateComment(commentId, commentVM, userId);

            if (updatedComment == null)
            {
                return NotFound(ErrorFormat.GetErrorJsonType(NotFound().StatusCode, ErrorNamesForMessages.CommentNotFound));
            }

            return Ok(updatedComment);
        }



        //the author of comment can delete his comment
        //the author of article can delete all comments on his article
        [HttpDelete("{id}/delete-comment/{commentId}")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> DeleteComment(int id, int commentId)
        {
            var userId = Request.Cookies["userId"];

            if (userId is null)
            {
                return BadRequest(ErrorFormat.GetErrorJsonType(BadRequest().StatusCode, ErrorNamesForMessages.ProblemWithUser));
            }

            var article = await _service.GetArtcileById(id);

            if (article is null)
            {
                return NotFound(ErrorFormat.GetErrorJsonType(NotFound().StatusCode, ErrorNamesForMessages.ArticleNotFound));
            }

            bool authorOrNot = false;

            if (article.UserId == userId)
            {
                authorOrNot = true;
            }

            await _commentsService.DeleteComment(userId, commentId, authorOrNot);

            return Ok("Comment deleted");
        }


        //all users can report any article
        [HttpPost("report-article/{id}")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> ReportArticle(int id, [FromBody] ReportedArticleVM reportedArticleVM)
        {
            var userId = Request.Cookies["userId"];
            var article = await _service.GetArtcileById(id);

            if (userId is null)
            {
                return BadRequest(ErrorFormat.GetErrorJsonType(BadRequest().StatusCode, ErrorNamesForMessages.ProblemWithUser));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ValidationState);
            }

            if (article is null)
            {
                return NotFound(ErrorFormat.GetErrorJsonType(NotFound().StatusCode, ErrorNamesForMessages.ArticleNotFound));
            }

            await _reportedArticlesService.ReportArticle(id, userId, reportedArticleVM);
            return Ok("Chosen article's reported");
        }

        //Report comment feature will be added 
        //Sorry for the inconvenience :)
        //public async Task<IActionResult> ReportComment() { }
        //I guess there is no column or table for reported comments. Also have to be added
    }
}
