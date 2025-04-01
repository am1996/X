using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.Models;
using X.Services;

namespace X.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(UserManager<User> userManager,LiteService liteService,JWTGenerator jwtGenerator) : ControllerBase
{
    private readonly UserManager<User> _userManager= userManager;
    private readonly LiteService _db = liteService;
    private readonly IJwtGenerator _jwtGenerator = jwtGenerator;


    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("logout")]
    public ActionResult Logout(){
        string? token = Request.Headers.Authorization;
        if (token == null) return BadRequest("Token not found.");
        token = token.Split(" ")[1];
        _db.Remove(token);
        return Ok($"Logged out successfully.");
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
        if(us[0].Email ==null) return NotFound("User Not Found.");
        return Ok(us);
    }
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginUser user){
        User? u = await _userManager.FindByEmailAsync(user.Email);
        if(u == null) return NotFound("User Not Found.");
        if(await _userManager.CheckPasswordAsync(u, user.Password)){
            string jwtToken = _jwtGenerator.GenerateJwtToken(u, "User");
            _db.Add(jwtToken, u.Id, DateTime.Now.AddDays(90)); //30days till token expires same as in JWTGenerator.cs
            return Ok(new{ jwtToken, message = "Login Successful"});
        }
        return Ok("Wrong Password Entered, Try Again.");
    }

    // POST: api/user
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterUser model)
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
            return Ok(new{message = "User created successfully", user = new{
                username = user.UserName,
                email = user.Email,
                firstname = user.FirstName,
                lastname = user.LastName,
                address = model.Address
            }});
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
    public async Task<IActionResult> Delete(string id)
    {
        User? u = await _userManager.FindByIdAsync(id);
        if(u != null){
            await _userManager.DeleteAsync(u);
            return Ok("User Deleted Successfully.");
        }
        return NotFound("User Not Found.");
    }
}