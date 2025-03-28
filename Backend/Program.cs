using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RocksDbSharp;
using Serilog;
using X.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ Read JWT settings correctly
var jwtSettings = new {
    Secret = builder.Configuration["JwtSettings:Secret"] ?? throw new Exception("Secret Key not found"),
    Issuer = builder.Configuration["JwtSettings:Issuer"] ?? throw new Exception("Issuer not found"),
    Audience = builder.Configuration["JwtSettings:Audience"] ?? throw new Exception("Audience not found"),
};

string ConnectionString = builder.Configuration.GetConnectionString("MySQLConnectionString")!;
string dbPath = Path.Combine(AppContext.BaseDirectory, builder.Configuration["RocksDB:Filename"]!);

// ✅ Configure Rocks.db
builder.Services.AddSingleton(provider =>
{
    var options = new DbOptions().SetCreateIfMissing(true);
    return RocksDb.Open(options, "rocksdb_data"); // Database directory
});

// ✅ Configure Serilog for Logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Console()
    .CreateLogger();

// ✅ Register Services
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton<JWTGenerator>();
builder.Logging.AddSerilog(Log.Logger);
builder.Services.AddSingleton<IRocksService,RocksService>();
// ✅ Configure Authentication
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret!))
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context => {
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("{ \"error\": \"Invalid token\" }");
            },
            OnChallenge = context =>
            {
                context.HandleResponse(); // ✅ Prevents redirect loops
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("{ \"error\": \"Unauthorized - Token required\" }");
            },
            OnMessageReceived = context => {
                var dbService = context.HttpContext.RequestServices.GetRequiredService<IRocksService>();
                string? token = context.Request.Headers.Authorization;
                string? userId = context.Principal?.FindFirst(ClaimTypes.Name)?.Value;
                if(userId != null && token != null){
                    string? dbToken = dbService.Get(userId);
                    if(dbToken == null || dbToken != token){
                        context.Fail("Unauthorized - Invalid Token");
                    }
                }   
                return Task.CompletedTask;
            }
        };
    }
);

builder.Services.AddAuthorization();
builder.Services.AddRouting();
builder.Services.AddControllers();

// ✅ Configure Database
builder.Services.AddDbContext<XContext>(options =>
    options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString)));

// ✅ Configure Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<XContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

app.UseRouting(); 
app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers();

app.Run();
