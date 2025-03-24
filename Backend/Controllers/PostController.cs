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
        public void Post()
        {          
            Log.Information(Request.Headers.Authorization!);
        }
        // GET: api/post
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            Log.Debug("JWT Auth Token: "+ _configuration["JwtSettings:Secret"] ?? "Secret Key not found");
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
    }
}