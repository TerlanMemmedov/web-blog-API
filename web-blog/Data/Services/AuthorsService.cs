//using Microsoft.EntityFrameworkCore;
//using web_blog.Data.Models;
//using web_blog.Data.ViewModels;

//namespace web_blog.Data.Services
//{
//    public class AuthorsService
//    {
//        private readonly AppDbContext _context;
//        private readonly AuthenticationService _authenticationService;

//        public AuthorsService(AppDbContext context, AuthenticationService authenticationService)
//        {
//            _context = context;
//            _authenticationService = authenticationService;
//        }

//        public async Task<List<Author>> GetAllAuthors()
//        {
//            var authors = await _context.Authors.ToListAsync();
//            return authors;
//        }

//        public async Task<Author> GetAuthorById(int id)
//        {
//            var author = await _context.Authors.FirstOrDefaultAsync(n => n.Id == id);
//            return author;
//        }

//        public async Task<Author> GetAuthorByUserIdAsync(string id)
//        {
//            var author = await _context.Authors.FirstOrDefaultAsync(n => n.UserId == id);
//            return author;
//        }

//        //only users after registraton can be author
//        //can be used with authorization 
//        public async Task AddAuthor(AuthorVM author, string id)
//        {
//            var _author = new Author()
//            {
//                NickName = author.NickName,
//                UserId = id
//            };

//            await _context.Authors.AddAsync(_author);
//            await _context.SaveChangesAsync();

//            await _authenticationService.ChangeToAuthorYourProfile(id);
//        }


//        public async Task<Author> UpdateAuthor(int id, AuthorVM author)
//        {
//            var _author = await _context.Authors.FirstOrDefaultAsync(n => n.Id == id);

//            if (_author != null)
//            {
//                _author.NickName = author.NickName;
//                await _context.SaveChangesAsync();
//            }
//            return _author;
//        }
//    }
//}
