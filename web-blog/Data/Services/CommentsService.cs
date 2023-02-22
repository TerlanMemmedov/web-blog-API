using Microsoft.EntityFrameworkCore;
using web_blog.Data.Models;
using web_blog.Data.ViewModels;

namespace web_blog.Data.Services
{
    public class CommentsService
    {
        private readonly AppDbContext _context;

        public CommentsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> GetCommentById(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(n => n.Id == id);

            return comment;
        }

        public async Task PostComment(int id, CommentVM comment, string userId)
        {
            var _comment = new Comment()
            {
                CommentText = comment.Text,
                UserId = userId,
                ArticleId = id,
                DateAdded = DateTime.Now
            };

            await _context.Comments.AddAsync(_comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteComment(string userId, int commentId, bool AuthorOrNot)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(n => n.Id == commentId);

            if (comment != null)
            {
                if (comment.UserId == userId || AuthorOrNot)
                {
                    //Delete comment
                    _context.Comments.Remove(comment);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
