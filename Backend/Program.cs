using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
builder.Services.AddSingleton<LiteService>();
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
            OnMessageReceived = context => {
                string? token = context.Request.Headers.Authorization.FirstOrDefault();
                if(token == null || token.Contains("Bearer") == false){
                    context.Fail("Unauthorized - Token required."); // ✅ Use plain string
                }
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                string? token = context.Request.Headers.Authorization.FirstOrDefault();
                token = token?.Substring("Bearer ".Length).Trim() ?? string.Empty;
                LiteService _db = context.HttpContext.RequestServices.GetRequiredService<LiteService>();
                string? tokenFetchedFromLite = _db.Get(token!) ?? null;
                if(string.IsNullOrEmpty(tokenFetchedFromLite)){
                    context.Fail("Please Login, token expired.");
                }

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                context.NoResult(); // ✅ Prevents writing response twice
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("{ \"error\": \"Invalid token\" }");
            },
            OnForbidden = context =>
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("{ \"error\": \"UnAuthroized - Token required\" }");
            },
            OnChallenge = context =>{
                if (context.Response.HasStarted) {
                    return Task.CompletedTask;
                }
                    
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                
                // Make sure this completes properly
                var result = context.Response.WriteAsync("{ \"error\": \"Unauthorized - Token required\" }");
                return result;
            },
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
