using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using X.Models;

namespace X.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController(XContext xContext,IConfiguration configuration) : ControllerBase
    {
        private readonly XContext _x_context= xContext;
        private readonly IConfiguration _configuration=configuration;

        // POST: api/post
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public ActionResult Post(Post post)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("User not found in claims");
            post.UserId = userId;
            _x_context.Posts.Add(post);
            _x_context.SaveChanges();
            return Ok(post);
        }
        // GET: api/post
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            List<Post> posts = [.. _x_context.Posts];
            return Ok(posts);
        }

        // GET: api/post/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            Post? post = _x_context.Posts.Find(id);
            if(post == null) return NotFound();
            return Ok(post);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public ActionResult<string> Delete(int id)
        {
            Post? post = _x_context.Posts.Find(id);
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;
            if(post == null) return NotFound();
            if(post.UserId != userId) return Unauthorized("You are not authorized to delete this post.");
            _x_context.Posts.Remove(post);
            _x_context.SaveChanges();
            return Ok(new {message="Post deleted successfully"});
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{id}")]
        public ActionResult<string> Put(int id, Post post)
        {
            Post? existingPost = _x_context.Posts.Find(id);
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;
            if(existingPost == null) return NotFound();
            if(existingPost.UserId != userId) return Unauthorized("You are not authorized to update this post.");
            existingPost.Title = post.Title;
            existingPost.Content = post.Content;
            existingPost.UpdatedAt = DateTime.Now;
            _x_context.SaveChanges();
            return Ok(existingPost);
        }
    }
}