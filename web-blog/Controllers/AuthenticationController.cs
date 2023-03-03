using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;                
using System.IdentityModel.Tokens.Jwt;       
using System.Security.Claims;            
using System.Text;
using web_blog.Data;
using web_blog.Data.Models;
using web_blog.Data.ViewModels.Authentication;
using System.CodeDom.Compiler;

namespace web_blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext context,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _configuration = configuration;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterVM reg)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Provide all information");
            }

            var userMail = await _userManager.FindByEmailAsync(_userManager.NormalizeEmail(reg.Email));

            if (userMail != null)
            {
                return BadRequest($"{reg.Email} is already exist");
            }

            ApplicationUser user = new ApplicationUser()
            {
                UserName = reg.UserName,
                Email = reg.Email,
                About = reg.About,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var sup = await _userManager.CreateAsync(user, reg.Password);

            if (!sup.Succeeded)
            {
                return BadRequest("Something happened on the way. Sorry, we can't register you. Try again");
            }


            //auto asigning user
            await _userManager.AddToRoleAsync(user, UserRoles.User);


            //There is no active role rather than just user, so this part is closed for now
            //switch (reg.Role)
            //{
            //    case "Admin":
            //        await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            //        break;
            //    //case "Author":
            //    //    await _userManager.AddToRoleAsync(user, UserRoles.Author);
            //    //    break;
            //    case "User":
            //        await _userManager.AddToRoleAsync(user, UserRoles.User);
            //        break;
            //    case null:
            //        await _userManager.AddToRoleAsync(user, UserRoles.User);
            //        break;
            //    default:
            //        await _userManager.AddToRoleAsync(user, UserRoles.User);
            //        break;
            //}


            return Created(nameof(Register), $"{reg.Email} is created");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginVM log)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Provide all information");
            }

            var user = await _userManager.FindByEmailAsync(log.Email);

            if (user != null)
            {
                if (await _userManager.CheckPasswordAsync(user, log.PassWord))
                {
                    if (await _userManager.GetLockoutEnabledAsync(user)) //true if not blocked
                    {
                        var token = await GenerateToken(user);

                        return Ok(token);
                    }
                    else // false if account blocked
                    {
                        return BadRequest("Your profile has been blocked for 3 days !  I have to write a new error json type here");
                    }
                }
                else
                {
                    return BadRequest($"Your password is wrong for {user.Email} account");
                }
            }

            return Unauthorized($"There is no profile with that email. Try again or remember your true email :)");
        }


        public async Task<AuthForm> GenerateToken(ApplicationUser user)
        {
            var authClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var item in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, item));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddDays(7),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            //Return it on session or with HttpOnly cookies 
            //HttpContext.Session.SetString("JWToken", jwtToken);
            HttpContext.Session.SetString("UserId", user.Id);
            HttpContext.Session.SetString("JWToken", jwtToken);


            HttpContext.Response.Cookies.Append("token", jwtToken, new CookieOptions()
            {
                Expires = token.ValidTo,
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            });
            HttpContext.Response.Cookies.Append("userId", user.Id, new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            });




            var auth = new AuthForm()
            {
                Token = jwtToken,
                ExpireTime = token.ValidTo
            };

            return auth;
        }
    }
}
