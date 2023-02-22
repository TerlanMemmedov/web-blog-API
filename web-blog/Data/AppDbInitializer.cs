using Microsoft.AspNetCore.Identity;
using web_blog.Data.Enums;
using web_blog.Data.Models;
using web_blog.Data.ViewModels.Authentication;

namespace web_blog.Data
{
    public class AppDbInitializer
    {
        //public static void Seed(IApplicationBuilder applicationBuilder)
        //{
        //    using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        //    {
        //        var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

        //        context.Database.EnsureCreated();

        //        if (!context.Authors.Any())
        //        {
        //            context.Authors.Add(new Author()
        //            {
        //                NickName = "Admiral",
        //            });
        //            context.SaveChanges();
        //        }
        //        if (!context.Articles.Any())
        //        {
        //            context.Articles.Add(new Article()
        //            {
        //                Title = "Sh",
        //                ArticleText = "Hello everyone, this is my project",
        //                Categories = Categories.Writing,
        //                DateAdded = DateTime.Now,
        //                AuthorId = 1
        //            });
        //            context.SaveChanges();
        //        }
        //    }
        //}

        public static async Task SeedRoles(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                //if (!await roleManager.RoleExistsAsync(UserRoles.Author))
                //    await roleManager.CreateAsync(new IdentityRole(UserRoles.Author));

                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }
        }
    }
}
