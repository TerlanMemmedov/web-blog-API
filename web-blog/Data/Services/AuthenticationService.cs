//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;
using web_blog.Data.Models;

namespace web_blog.Data.Services
{
    public class AuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext context,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
        }

        //public async Task ChangeToAuthorYourProfile(string id)
        //{
        //    var user = await _userManager.FindByIdAsync(id);

        //    if (user != null)
        //    {
        //        await _userManager.RemoveFromRoleAsync(user, "User");

        //        await _userManager.AddToRoleAsync(user, "Author");

        //        _context.SaveChanges();
        //    }
        //}

        //public async Task UpdateUserProfile(int userId, )
        //{

        //}
    }
}
