using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);
var jwtSettings = new {
    Secret = builder.Configuration["JwtSettings:Secret"] ?? throw new Exception("Secret Key not found"),
    Issuer = builder.Configuration["JwtSettings:Issuer"] ?? throw new Exception("Issuer not found"),
    Audience = builder.Configuration["JwtSettings:Audience"] ?? throw new Exception("Audience not found"),
};
string ConnectionString = builder.Configuration.GetConnectionString("MySQLConnectionString")!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters{
        ValidateAudience =true,
        ValidateLifetime=true,
        ValidateIssuer=true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Secret,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret!))
    };
});
builder.Services.AddAuthorization();
builder.Services.AddRouting();
builder.Services.AddControllers();
builder.Services.AddDbContext<XContext>(options =>
    options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString)));
    
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<XContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton<JWTGenerator>();

var app = builder.Build();


app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
