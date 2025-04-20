using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace X.Models;
public class Comment{
    public int Id { get; set; }
    
    [Required,MaxLength(500),MinLength(5)]
    public required string Content { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    [ForeignKey("Post")]
    public required int PostId { get; set; }
    [Required,ForeignKey("User")]
    public required string UserId { get; set; }
    public DateTime UpdatedAt{get;set;}
    public virtual Post? Post { get; set; } // Navigation to the Post
    public virtual User? User { get; set; } // Navigation to the User
}