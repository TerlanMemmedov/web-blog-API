using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web_blog.Data.Services;
using web_blog.Data.ViewModels.Authentication;

namespace web_blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly ReportedArticlesService _reportedArticlesService;

        public ReportsController(ReportedArticlesService reportedArticlesService)
        {
            _reportedArticlesService = reportedArticlesService;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> GetReports()
        {
            var userId = Request.Cookies["userId"];

            if (userId is null)
            {
                return BadRequest("Problem with User");
            }

            var reports = await _reportedArticlesService.GetAllReportedArticlesOfUser(userId);

            if (reports.Count == 0 || reports == null)
            {
                return Ok("There is no reports ");
            }

            return Ok(reports);

            //maybe here combine the reported articles and comments and send them inside a dictionary
        }
    }
}
