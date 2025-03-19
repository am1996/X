using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public UserController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    // GET: api/user
    [HttpGet]
    public ActionResult<IEnumerable<string>> Get()
    {
        List<User> users = [.._userManager.Users];
        var us = users.Select(u => new{
            u.UserName,
            u.Email,
            FirstName = u.FirstName ?? "N/A",
            LastName = u.LastName ?? "N/A",
            Address = u.Address ?? "N/A"
        }).ToList();
        return Ok(us);
    }

    // GET: api/user/5
    [HttpGet("{id}")]
    public async Task<ActionResult<string>> Get(string id)
    {
        try{
            User? user = await _userManager.FindByIdAsync(id);
            var u = new{
                user?.UserName,
                user?.Email,
                FirstName = user?.FirstName ?? "N/A",
                LastName = user?.LastName ?? "N/A",
                Address = user?.Address ?? "N/A"
            };
            return Ok(u);
        }catch(Exception){
            return NotFound();
        }
    }

    // POST: api/user
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] RegisterUser model)
    {
        if(_userManager.Users.Any(u => u.UserName == model.UserName || u.Email == model.Email)){
            return BadRequest(new{message = "Username or Email already exists"});
        }
        User user = new(){
            UserName = model.UserName,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Address = model.Address
        };
        IdentityResult result = await _userManager.CreateAsync(user, model.Password);
        if(result.Succeeded){
            return Ok(new{message = "User created successfully", user});
        }else{
            return BadRequest("One of the fields is missing.");
        }
    }

    // PUT: api/user/5
    [HttpPut("{id}")]
    public void Put(string id, [FromBody] string value)
    {
    }

    // DELETE: api/user/5
    [HttpDelete("{id}")]
    public void Delete(string id)
    {
    }
}