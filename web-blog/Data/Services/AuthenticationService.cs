//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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


        public async Task IncreaseAllAndLastOnes(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(n => n.Id == userId);

            if (user != null)
            {
                user.NumberOfAllReportedThings++;
                user.NumberOfLastReportedThings++;

                await _context.SaveChangesAsync();
            }
        }


        //blocks for last reported above 5 and also will open blocks blocked users again
        public async Task BlockUsersAcceptedReportedPlusFive()
        {
            var users = await _context.Users.ToListAsync();
            //var users = _userManager.Users.ToList();

            foreach (var user in users)
            {
                // changed to new column number of the reported things have
                if (user.NumberOfLastReportedThings >= 5) 
                {
                    user.NumberOfLastReportedThings = 0;
                    user.LockoutEnabled = false;
                    user.LockoutEnd = DateTime.Now.AddDays(3);

                    //if the profile is blocked need to delete token from the browser's session in frontend

                    await _context.SaveChangesAsync();
                }
            }

            //open blocks
            var blockedUsers = await _context.Users.Where(n => n.LockoutEnabled == false).ToListAsync();

            foreach (var blockedUser in blockedUsers)
            {
                if (blockedUser.LockoutEnd <= DateTime.Now)
                {
                    blockedUser.LockoutEnd = null;
                    blockedUser.LockoutEnabled = true;

                    await _context.SaveChangesAsync();
                }
            }

        }
    }
}
