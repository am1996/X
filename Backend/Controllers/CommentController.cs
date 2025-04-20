using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using X.Models;

namespace X.Controllers
{
    public class CommentController(XContext xContext,IConfiguration configuration): ControllerBase
    {
        private readonly XContext _x_context= xContext;
        private readonly IConfiguration _configuration=configuration;

        // POST: api/comment
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public ActionResult Post(Comment comment)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("User not found in claims");
            comment.UserId = userId;
            _x_context.Comments.Add(comment);
            _x_context.SaveChanges();
            return Ok(comment);
        }
        // GET: api/comment
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            List<Comment> comments = [.. _x_context.Comments];
            return Ok(comments);
        }

        // GET: api/comment/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            Comment? comment = _x_context.Comments.Find(id);
            if(comment == null) return NotFound();
            return Ok(comment);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public ActionResult<string> Delete(int id){
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("User not found in claims");
            Comment? comment = _x_context.Comments.Find(id);
            if(comment == null) return NotFound();
            if(comment.UserId != userId) return Unauthorized("You are not authorized to delete this comment.");
            _x_context.Comments.Remove(comment);
            _x_context.SaveChanges();
            return Ok(new {message="Comment deleted successfully"});
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}/update")]
        public ActionResult<string> Put(int id, Comment comment){
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("User not found in claims");
            Comment? existingComment = _x_context.Comments.Find(id);
            if(existingComment == null) return NotFound();
            if(existingComment.UserId != userId) return Unauthorized("You are not authorized to update this comment.");
            existingComment.Content = comment.Content;
            _x_context.SaveChanges();
            return Ok(new {message="Comment updated successfully"});
        }
    }
}