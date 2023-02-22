﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using web_blog.Data.Services;
using web_blog.Data.ViewModels.Authentication;

namespace web_blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly ReportedArticlesService _reportedArticlesService;
        private readonly ArticlesService _articlesService;

        public AdminController(ReportedArticlesService reportedArticlesService, 
            ArticlesService articlesService)
        {
            _reportedArticlesService = reportedArticlesService;
            _articlesService = articlesService;
        }

        //Getting reported articles
        [HttpGet("reported-articles")]
        public async Task<IActionResult> GetReportedArticles()
        {
            var reportedArticles = await _reportedArticlesService.GetAllReportedArticles();

            if (reportedArticles.Count == 0 || reportedArticles == null)
            {
                return Ok("There is no reported article");
            }

            return Ok(reportedArticles);
        }

        [HttpGet("reported-article/{id}")]
        public async Task<IActionResult> GetReportedArticles(int id)
        {
            var reportedArticle = await _reportedArticlesService.GetReportedArticleById(id);

            if (reportedArticle is null)
            {
                return NotFound("Problem with article. Maybe it's deleted");
            }

            return Ok(reportedArticle);
        }

        [HttpPost("reported-article/{id}/accept")]
        public async Task<IActionResult> AcceptReport(int id)
        {
            //change the report status to accepted: code 1/
            const string statusMessage = "accepted";

            await _reportedArticlesService.ChangeStatus(id, statusMessage);

            //delete article with given id

            int articleId = await _reportedArticlesService.GetArticleIdByReportId(id);

            await _articlesService.DeleteArticleByAdmin(articleId);

            return Ok("Report " + statusMessage + "\n" + "Article deleted");
        }

        [HttpPost("reported-article/{id}/decline")]
        public async Task<IActionResult> DeclineReport(int id)
        {
            //change the report status to accepted: code 1/
            const string statusMessage = "declined";

            await _reportedArticlesService.ChangeStatus(id, statusMessage);

            return Ok("Report " + statusMessage);
        }
    }
}