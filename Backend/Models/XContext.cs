using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using X.Models;

public class XContext : IdentityDbContext<User>
{
    public XContext(DbContextOptions<XContext> options) : base(options)
    {
    }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Dislike> Dislikes { get; set; }
}