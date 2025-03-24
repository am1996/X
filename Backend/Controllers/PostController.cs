using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using X.Models;

namespace X.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly XContext _x_context;
        private readonly IConfiguration _configuration;
        public PostController(XContext xContext,IConfiguration configuration)
        {
            _x_context = xContext;
            _configuration = configuration;
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

        // POST: api/post
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/post/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/post/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}