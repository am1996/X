using Microsoft.AspNetCore.Mvc;
using X.Models;

namespace X.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly XContext _x_context;
        public PostController(XContext xContext)
        {
            _x_context = xContext;
        }
        // GET: api/post
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            List<Post> posts = [.. _x_context.Posts];
            return Ok(posts);
        }

        // GET: api/post/5
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