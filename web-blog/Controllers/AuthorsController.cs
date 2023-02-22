//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Net.Http.Headers;
//using System.Net;
//using web_blog.Data.Models;
//using web_blog.Data.Services;
//using web_blog.Data.ViewModels;

//namespace web_blog.Controllers  //will change to work on profiles
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthorsController : ControllerBase
//    {
//        private readonly AuthorsService _service;

//        public AuthorsController(AuthorsService service)
//        {
//            _service = service;
//        }

//        //all can
//        [HttpGet("")]
//        public async Task<IActionResult> GetAllAuthors()
//        {
//            var authors = await _service.GetAllAuthors();

//            if (authors is null)
//            {
//                return BadRequest();
//            }

//            return Ok(authors);
//        }

//        //all can
//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetAuthor(int id)
//        {
//            var author = await _service.GetAuthorById(id);

//            if (author is null)
//            {
//                return BadRequest();
//            }

//            return Ok(author);
//        }

//        //authorization("User" role) only thmeselves can change to the author
//        [Authorize(Roles = "User")]
//        [HttpPost("change-to-author")]
//        public async Task<IActionResult> BecomeToAuthor([FromBody] AuthorVM author)
//        {
//            var cookId = Request.Cookies["userId"];

//            if (cookId is null)
//            {
//                return BadRequest("Problem with user id");
//            }

//            await _service.AddAuthor(author, cookId);
//            return NoContent();
//        }

//        //authorization(all roles) only thmeselves can change to the author
//        [HttpPut("update-author-profile")]
//        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] AuthorVM author)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest();
//            }

//            var _author = await _service.UpdateAuthor(id, author);

//            if (_author is null)
//            {
//                return BadRequest();
//            }

//            return Ok(_author);
//        }

        

//    }
//}
