using Microsoft.EntityFrameworkCore;
using web_blog.Data.Models;
using web_blog.Data.ViewModels;

namespace web_blog.Data.Services
{
    public class EmojisService
    {
        private readonly AppDbContext _context;

        public EmojisService(AppDbContext context)
        {
            _context = context;
        }


        //one problem with when out of range in emoji types/ just prints number no error
        public async Task PostEmoji(int id, EmojiVM emoji, string userId)
        {
            Emoji emojiIn = await _context.Emojis.Where(n => n.UserId == userId && n.ArticleId == id).FirstOrDefaultAsync();

            if (emojiIn is null)
            {
                var _emoji = new Emoji()
                {
                    ArticleId = id,
                    UserId = userId,
                    EmojiTypes = emoji.emojiType
                };

                await _context.Emojis.AddAsync(_emoji);
                await _context.SaveChangesAsync();
            }
            else
            {
                if (emojiIn.UserId == userId)
                {
                    if (emojiIn.EmojiTypes == emoji.emojiType)
                    {
                        //Delete
                        _context.Emojis.Remove(emojiIn);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        //update
                        emojiIn.EmojiTypes = emoji.emojiType;
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
