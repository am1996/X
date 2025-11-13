using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.Models;
using X.Services;

namespace X.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(UserManager<User> userManager,LiteService liteService,JWTGenerator jwtGenerator) : ControllerBase
{
    private readonly UserManager<User> _userManager= userManager;
    private readonly LiteService _db = liteService;
    private readonly IJwtGenerator _jwtGenerator = jwtGenerator;

    // Get: Auth Check
    [HttpGet("authcheck")]
    public ActionResult<string> AuthCheck()
    {
        string? token = Request.Cookies["X-Access-Token"];
        if (token == null) return BadRequest("Token not found.");
        token = token.Split(" ")[1];
        if (_db.Get(token) is not null){
            return Ok("authenticated");
        }
        return Unauthorized("User is not authenticated.");
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("logout")]
    public ActionResult Logout(){
        string? token = Request.Cookies["X-Access-Token"];
        if (token == null) return BadRequest("Token not found.");
        token = token.Split(" ")[1];
        _db.Remove(token);
        Response.Cookies.Append("X-Access-Token", "", new CookieOptions
        {
            Expires = DateTime.Now.AddDays(-1),
            HttpOnly = true,
        });
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
            _db.Add(jwtToken, u.Id, DateTime.Now.AddDays(90)); //30 days till token expires same as in JWTGenerator.cs
            
            Response.Cookies.Append("jwt", jwtToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Only true if using HTTPS
                SameSite = SameSiteMode.None,
            });
            return Ok(new { message = "Login Successful" });
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

    // PUT: api/user/update
    [HttpPut("update")]
    public ActionResult Put(Dictionary<string,string> Data)
    {
        string key = Data.Select(x => x.Key).FirstOrDefault() ?? throw new Exception("Key not found in dictionary");
        string value = Data.Select(x => x.Value).FirstOrDefault() ?? throw new Exception("Value not found in dictionary");
        string id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;
        User? u = _userManager.FindByIdAsync(id).Result;
        if(u != null){
            switch(key.ToLower()){
                case "username":
                    u.UserName = value;
                    break;
                case "email":
                    u.Email = value;
                    break;
                case "firstname":
                    u.FirstName = value;
                    break;
                case "lastname":
                    u.LastName = value;
                    break;
                case "address":
                    u.Address = value;
                    break;
                default:
                    return BadRequest("Invalid Key.");
            }
            _userManager.UpdateAsync(u);
            return Ok(new{user=u, message = "User Updated Successfully"});
        }
        return NotFound("User Not Found.");
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